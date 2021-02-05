namespace Gradebook.Web.Tests.ServicesTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using FluentAssertions;
    using Gradebook.Data.Common.Repositories;
    using Gradebook.Data.Models;
    using Gradebook.Data.Models.Grades;
    using Gradebook.Web.Areas.Teacher.ViewModels.InputModels;
    using Gradebook.Web.Services;
    using Gradebook.Web.Services.Interfaces;
    using Gradebook.Web.ViewModels.InputModels;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class GradesServiceTests
    {
        private const int StudentSubjectId = 13;
        private const int StudentCorrectId = 5;
        private const int SubjectCorrectId = 4;
        private const int TestGradeDBId = 1;
        private const int TestTeacherDBId = 10;
        private const string TestTeacherUniqueID = "T021ppzz";

        private Mock<IDeletableEntityRepository<Grade>> _gradesRepositoryMock;
        private Mock<IRepository<Teacher>> _teachersRepositoryMock;
        private Mock<IRepository<StudentSubject>> _studentSubjectsRepositoryMock;

        private IGradesService _gradesService;

        [SetUp]
        public void Setup()
        {
            _gradesRepositoryMock = new Mock<IDeletableEntityRepository<Grade>>();
            _teachersRepositoryMock = new Mock<IRepository<Teacher>>();
            _studentSubjectsRepositoryMock = new Mock<IRepository<StudentSubject>>();
            _gradesService = new GradesService(
                _gradesRepositoryMock.Object,
                _teachersRepositoryMock.Object,
                _studentSubjectsRepositoryMock.Object);
        }

        [Test]
        public async Task CreateGradeAsync_HappyPath()
        {
            OneTimeSetUp();
            var newAbsence = new GradeInputModel()
            {
                Period = GradePeriod.FirstTerm,
                Type = GradeType.Final,
                TeacherId = TestTeacherDBId,
                StudentId = StudentCorrectId,
                SubjectId = SubjectCorrectId,
            };

            await _gradesService.CreateAsync(newAbsence);
            _gradesRepositoryMock.Object.All().Count().Should().Be(1);
        }

        [Test]
        public async Task CreateGradeAsync_WhenStudentSubjectNull_ShouldHandleException()
        {
            _teachersRepositoryMock.Setup(t => t.All())
                 .Returns(new List<Teacher> { }.AsQueryable());
            var newGrade = new GradeInputModel()
            {
                StudentId = StudentSubjectId,
                Period = GradePeriod.FirstTerm,
                Type = GradeType.Final,
                TeacherId = TestTeacherDBId,
                SubjectId = 10,
            };
            try
            {
                await _gradesService.CreateAsync(newGrade);
            }
            catch (ArgumentException ae)
            {
                ae.Message.Should().Be($"Sorry, we couldn't find pair of student ({StudentSubjectId}) and subject({10})");
            }
        }

        [Test]
        public async Task CreateGradeAsync_WhenTeacherNull_ShouldHandleException()
        {
            OneTimeSetUp();

            _teachersRepositoryMock.Setup(t => t.All())
                 .Returns(new List<Teacher> { }.AsQueryable());
            var newGrade = new GradeInputModel()
            {
                StudentId = 5,
                Period = GradePeriod.FirstTerm,
                Type = GradeType.Final,
                TeacherId = TestTeacherDBId,
                SubjectId = 4,
            };
            try
            {
                await _gradesService.CreateAsync(newGrade);
            }
            catch (ArgumentException ae)
            {
                ae.Message.Should().Be($"Sorry, we couldn't find teacher with id {TestTeacherDBId}");
            }
        }

        [Test]
        [TestCase(TestGradeDBId)]
        public async Task DeleteGradeAsync_HappyPath(int id)
        {
            OneTimeSetUp();

            await _gradesService.DeleteAsync(TestTeacherDBId);
            var expected = 0;

            _gradesRepositoryMock.Object.AllAsNoTracking().Count().Should().Be(expected);
        }

        [Test]
        public async Task EditAsyncTest()
        {
            OneTimeSetUp();
            var expectGrade = _gradesRepositoryMock.Object.All().FirstOrDefault();
            var newGrade = new GradeModifyInputModel()
            {
                Id = expectGrade.Id,
                Grade = new GradeInputModel()
                {
                    StudentId = expectGrade.StudentSubject.StudentId,
                    Period = GradePeriod.SecondTerm,
                    Type = GradeType.Normal,
                },
            };

            await _gradesService.EditAsync(newGrade);
            expectGrade.Should().NotBe(newGrade);
        }

        private void OneTimeSetUp()
        {
            _teachersRepositoryMock.Setup(t => t.All())
                   .Returns(new List<Teacher>
                   {
                    new Teacher
                    {
                        Id = TestTeacherDBId,
                        UniqueId = TestTeacherUniqueID
                    }
                   }.AsQueryable());

            _studentSubjectsRepositoryMock.Setup(s => s.All())
                .Returns(new List<StudentSubject>
                {
                    new StudentSubject()
                    {
                        StudentId = 5,
                        SubjectId = 4,
                        CreatedOn = DateTime.UtcNow,
                    }
                }.AsQueryable());

            _gradesRepositoryMock.Setup(a => a.All())
                .Returns(new List<Grade>
                {
                    new Grade()
                    {
                        Period = GradePeriod.FirstTerm,
                        Type = GradeType.Final,
                        Value = 5,
                        TeacherId = TestTeacherDBId,
                        Teacher = _teachersRepositoryMock.Object.All().FirstOrDefault(),
                        StudentSubjectId = StudentSubjectId,
                        StudentSubject = _studentSubjectsRepositoryMock.Object.All().FirstOrDefault(),
                    }
                }.AsQueryable());
        }
    }
}
