namespace Gradebook.Web.Tests.ServicesTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;
    using Gradebook.Data.Common.Repositories;
    using Gradebook.Data.Models;
    using Gradebook.Services.Data;
    using Gradebook.Services.Data.Interfaces;
    using Gradebook.Web.Services;
    using Gradebook.Web.Services.Interfaces;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class StudentsServiceTests
    {

        private const int StudentId = 1;
        private const int ParentId = 1;
        private const string StudentUniqueId = "S021637479510198977104";
        private const string ParentUniqueId = "P021637480842091823618";

        private Mock<IDeletableEntityRepository<Student>> _studentsRepositoryMock;
        private Mock<IDeletableEntityRepository<School>> _schoolsRepositoryMock;
        private Mock<IDeletableEntityRepository<Parent>> _parentsRepositoryMock;
        private Mock<IDeletableEntityRepository<StudentParent>> _studentParentsRepositoryMock;

        private IIdGeneratorService _idGeneratorService;
        private IStudentsService _studentsService;

        [SetUp]
        public void Setup()
        {
            _studentsRepositoryMock = new Mock<IDeletableEntityRepository<Student>>();
            _schoolsRepositoryMock = new Mock<IDeletableEntityRepository<School>>();
            _parentsRepositoryMock = new Mock<IDeletableEntityRepository<Parent>>();
            _studentParentsRepositoryMock = new Mock<IDeletableEntityRepository<StudentParent>>();
            _idGeneratorService = new IdGeneratorService();
            _studentsService = new StudentsService(
                _studentsRepositoryMock.Object,
                _schoolsRepositoryMock.Object,
                _parentsRepositoryMock.Object,
                _studentParentsRepositoryMock.Object,
                _idGeneratorService);
        }

        [Test]
        public void GetIdByUniqueIdTest()
        {
            OneTimeSetUp();
            var student = NewStudentCreate().Id;
            var result = _studentsService.GetIdByUniqueId(StudentUniqueId);
            student.Should().Be(result);
        }

        private void OneTimeSetUp()
        {
            _studentsRepositoryMock.Setup(t => t.All())
               .Returns(new List<Student>
               {
                    NewStudentCreate()
               }.AsQueryable());

            _parentsRepositoryMock.Setup(a => a.All())
                .Returns(new List<Parent>
                {
                    NewParentCreate()
                }.AsQueryable());

            _studentParentsRepositoryMock.Setup(s => s.All())
                .Returns(new List<StudentParent>
                {
                    new StudentParent()
                    {
                        StudentId = StudentId,
                        ParentId = ParentId,
                    }
                }.AsQueryable());
        }

        private Student NewStudentCreate()
        {
            return new Student
            {
                UniqueId = StudentUniqueId,
                FirstName = "Foncho",
                LastName = "Tarikata",
                Username = "QashabiFoncho",
                Email = "QashabiFoncho@abv.bg",
                BirthDate = DateTime.UtcNow.AddYears(-15),
                PersonalIdentificationNumber = "9521110000",
            };
        }

        private Parent NewParentCreate()
        {
            return new Parent
            {
                Id = ParentId,
                FirstName = "Dona",
                LastName = "Donkova",
                PhoneNumber = "0884243472",
                Email = "DonaDonkova@abv.bg",
                UniqueId = "P021637480842091823618",
                StudentParents = new List<StudentParent>()
                {
                    new StudentParent()
                    {
                        StudentId = StudentId,
                        ParentId = ParentId,
                    }
                }
            };
        }
    }
}
