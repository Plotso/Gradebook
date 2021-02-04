namespace Gradebook.Web.Tests.ServicesTests
{
    using FluentAssertions;
    using Gradebook.Data.Common.Repositories;
    using Gradebook.Data.Models;
    using Gradebook.Data.Models.Absences;
    using Gradebook.Web.Services;
    using Gradebook.Web.Services.Interfaces;
    using Gradebook.Web.ViewModels.InputModels;
    using Moq;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [TestFixture]
    public class AbsencesServiceTests
    {
        private const int StudentSubjectId = 13;
        private const int StudentCorrectId = 5;
        private const int SubjectCorrectId = 4;
        private const int StudentYearBorn = 13;
        private const int TestTeacherDBId = 10;
        private const int TestTeacherUncorrectId = 2;
        private const int TestAbsenceDBId = 4;
        private const string TestTeacherUniqueID = "T021ppzz";

        private Mock<IDeletableEntityRepository<Absence>> _absencesRepositoryMock;
        private Mock<IRepository<Teacher>> _teachersRepositoryMock;
        private Mock<IRepository<StudentSubject>> _studentSubjectsRepositoryMock;
        private Mock<IRepository<Student>> _studentRepositoryMock;

        private IAbsencesService _absencesService;

        [SetUp]
        public void Setup()
        {
            _absencesRepositoryMock = new Mock<IDeletableEntityRepository<Absence>>();
            _teachersRepositoryMock = new Mock<IRepository<Teacher>>();
            _studentSubjectsRepositoryMock = new Mock<IRepository<StudentSubject>>();
            _studentRepositoryMock = new Mock<IRepository<Student>>();
            _absencesService = new AbsencesService(
                _absencesRepositoryMock.Object,
                _teachersRepositoryMock.Object,
                _studentSubjectsRepositoryMock.Object);
        }

        [Test]
        public async Task CreateAbsenceAsync()
        {
            OneTimeSetUp();
            var newAbsence = new AbsenceInputModel()
            {
                Period = AbsencePeriod.FirstTerm,
                Type = AbsenceType.Full,
                TeacherId = TestTeacherDBId,
                StudentId = StudentCorrectId,
                SubjectId = SubjectCorrectId,
            };

            await _absencesService.CreateAsync(newAbsence);
            _absencesRepositoryMock.Object.All().Count().Should().Be(1);
        }

        [Test]
        public async Task CreateAbsenceAsync_WithoutPairOfStudentSubject_ShouldThrowException()
        {
            _teachersRepositoryMock.Setup(t => t.All())
                 .Returns(new List<Teacher> { }.AsQueryable());
            var newAbsence = new AbsenceInputModel()
            {
                StudentId = StudentSubjectId,
                Period = AbsencePeriod.FirstTerm,
                Type = AbsenceType.Full,
                TeacherId = TestTeacherDBId,
                SubjectId = 10,
            };
            try
            {
                await _absencesService.CreateAsync(newAbsence);
            }
            catch (ArgumentException ae)
            {
                ae.Message.Should().Be($"Sorry, we couldn't find pair of student ({StudentSubjectId}) and subject({10})");
            }
        }

        [Test]
        public async Task CreateAbsenceAsync_WithoutTeacherId_ShouldThrowException()
        {
            OneTimeSetUp();
            var newAbsence = new AbsenceInputModel()
            {
                StudentId = StudentCorrectId,
                Period = AbsencePeriod.FirstTerm,
                Type = AbsenceType.Full,
                TeacherId = TestTeacherUncorrectId,
                SubjectId = SubjectCorrectId,
            };
            try
            {
                await _absencesService.CreateAsync(newAbsence);
            }
            catch (ArgumentException ae)
            {
                ae.Message.Should().Be($"Sorry, we couldn't find teacher with id {TestTeacherUncorrectId}");
            }
        }



    [Test]
    [TestCase(TestAbsenceDBId)]
    public async Task DeleteAbsenceAsync(int id)
    {
        OneTimeSetUp();

        await _absencesService.DeleteAsync(TestTeacherDBId);
        var expected = 0;

        _absencesRepositoryMock.Object.AllAsNoTracking().Count().Should().Be(expected);

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

        _studentRepositoryMock.Setup(t => t.All())
            .Returns(new List<Student>
            {
                    new Student
                    {
                        FirstName = "Foncho",
                        LastName = "Tarikata",
                        Username = "QashabiFoncho",
                        Email = "QashabiFoncho@abv.bg",
                        BirthDate = DateTime.UtcNow.AddYears(-StudentYearBorn),
                        PersonalIdentificationNumber = "9521110000",
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

        _absencesRepositoryMock.Setup(a => a.All())
            .Returns(new List<Absence>
            {
                    new Absence()
                    {
                        Id = TestAbsenceDBId,
                        Period = AbsencePeriod.FirstTerm,
                        Type = AbsenceType.Full,
                        TeacherId = TestTeacherDBId,
                        Teacher = _teachersRepositoryMock.Object.All().FirstOrDefault(),
                        StudentSubjectId = StudentSubjectId,
                        StudentSubject = _studentSubjectsRepositoryMock.Object.All().FirstOrDefault(),
                    }
            }.AsQueryable());
    }
}
}
