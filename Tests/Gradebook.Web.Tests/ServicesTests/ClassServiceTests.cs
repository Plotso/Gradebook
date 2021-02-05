namespace Gradebook.Web.Tests.ServicesTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;
    using Gradebook.Data.Common.Repositories;
    using Gradebook.Data.Models;
    using Gradebook.Web.Services;
    using Gradebook.Web.Services.Interfaces;
    using Gradebook.Web.ViewModels.Classes;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class ClassServiceTests
    {
        private const int TestTeacherDBId = 1;
        private const int TestSchoolDBId = 1;
        private const string TestTeacherUniqueID = "T021clst";
        private const string TestSchoolName = "151";
        private const int StudentBornBefore = 15;

        private Mock<IDeletableEntityRepository<Class>> _classesRepositoryMock;
        private Mock<IDeletableEntityRepository<Student>> _studentsRepositoryMock;
        private Mock<IRepository<Teacher>> _teachersRepositoryMock;
        private Mock<IRepository<School>> _schoolsRepositoryMock;

        private IClassesService _classService;

        [SetUp]
        public void Setup()
        {
            _classesRepositoryMock = new Mock<IDeletableEntityRepository<Class>>();
            _studentsRepositoryMock = new Mock<IDeletableEntityRepository<Student>>();
            _teachersRepositoryMock = new Mock<IRepository<Teacher>>();
            _schoolsRepositoryMock = new Mock<IRepository<School>>();
            _classService = new ClassesService(
                _classesRepositoryMock.Object,
                _studentsRepositoryMock.Object,
                _teachersRepositoryMock.Object,
                _schoolsRepositoryMock.Object);
        }

        public void GetAllClassesByTeacherIdAsyncTest_ShouldReturnTeackerClasses()
        {
            OneTimeSetUpAsync();
            var allClassesOfCurrentTeacher = _classService
                .GetAllByTeacherId<Class>(TestTeacherDBId);
            allClassesOfCurrentTeacher.Should().NotBeEmpty();
        }

        private static Student NewStudentCreate()
        {
            return new Student
            {
                FirstName = "Foncho",
                LastName = "Tarikata",
                Username = "QashabiFoncho",
                Email = "QashabiFoncho@abv.bg",
                BirthDate = DateTime.UtcNow.AddYears(-StudentBornBefore),
                PersonalIdentificationNumber = "9521110000",
            };
        }

        private static Class CreateClass()
        {
            return new Class()
            {
                Letter = 'A',
                Students = new List<Student>()
                        {
                            NewStudentCreate()
                        }
            };
        }


        private void OneTimeSetUpAsync()
        {
            _schoolsRepositoryMock.Setup(t => t.All())
                   .Returns(new List<School>
                   {
                    new School
                    {
                        Id = TestSchoolDBId,
                        Name = TestSchoolName,
                    }
                   }.AsQueryable());

            _teachersRepositoryMock.Setup(t => t.All())
                   .Returns(new List<Teacher>
                   {
                    new Teacher
                    {
                        Id = TestTeacherDBId,
                        UniqueId = TestTeacherUniqueID
                    }
                   }.AsQueryable());

            _studentsRepositoryMock.Setup(t => t.All())
                .Returns(new List<Student>
                {
                    NewStudentCreate()
                }.AsQueryable());

            _classesRepositoryMock.Setup(t => t.All())
                .Returns(new List<Class>
                { CreateClass()
                }.AsQueryable());

            var newClass = CreateClass();
            _classesRepositoryMock.Object.AddAsync(newClass);
        }
    }
}
