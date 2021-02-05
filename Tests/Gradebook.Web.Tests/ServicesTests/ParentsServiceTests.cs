namespace Gradebook.Web.Tests.ServicesTests
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Gradebook.Data.Common.Repositories;
    using Gradebook.Data.Models;
    using Gradebook.Services.Data;
    using Gradebook.Services.Data.Interfaces;
    using Gradebook.Web.Services;
    using Gradebook.Web.Services.Interfaces;
    using Gradebook.Web.ViewModels.InputModels;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class ParentsServiceTests
    {
        private const int StudentId = 1;
        private const int ParentId = 1;
        private const string StudentUniqueId = "S021637479510198977104";
        private const string ParentUniqueId = "P021637480842091823618";

        private Mock<IDeletableEntityRepository<Parent>> _parentsRepositoryMock;
        private Mock<IRepository<Student>> _studentsRepositoryMock;
        private Mock<IDeletableEntityRepository<StudentParent>> _studentParentsRepositoryMock;

        private IIdGeneratorService _idGeneratorService;
        private IParentsService _parentsService;

        [SetUp]
        public void Setup()
        {
            _parentsRepositoryMock = new Mock<IDeletableEntityRepository<Parent>>();
            _studentsRepositoryMock = new Mock<IRepository<Student>>();
            _studentParentsRepositoryMock = new Mock<IDeletableEntityRepository<StudentParent>>();
            _idGeneratorService = new IdGeneratorService();
            _parentsService = new ParentsService(
                _parentsRepositoryMock.Object,
                _studentsRepositoryMock.Object,
                _studentParentsRepositoryMock.Object,
                _idGeneratorService);
        }

        [Test]
        public async Task CreateParentAsync_WithoutAnyStudentIds()
        {
            OneTimeSetUp();
            var parent = NewParentCreate();
            var newParentInputModel = new ParentInputModel()
            {
                FirstName = parent.FirstName,
                LastName = parent.LastName,
                PhoneNumber = parent.PhoneNumber,
                StudentIds = new List<string> { }
            };
            try
            {
                await _parentsService.CreateParentAsync<Parent>(newParentInputModel);
            }
            catch (ArgumentException ae)
            {
                ae.Message.Should().Be($"Sorry, we couldn't any student with id in the following list:  [{string.Join(", ", newParentInputModel.StudentIds)}]");
            }

            _parentsRepositoryMock.Object.All().Count().Should().Be(1);
        }

        //[Test]
        //[TypeConverter(typeof(Parent))]
        //public async Task CreateParentAsync_HappyPath<T>()
        //{
        //    OneTimeSetUp();
        //    var parent = NewParentCreate();
        //    var newParentInputModel = new ParentInputModel()
        //    {
        //        FirstName = parent.FirstName,
        //        LastName = parent.LastName,
        //        PhoneNumber = parent.PhoneNumber,
        //        StudentIds = new List<string> { "0" }
        //    };

        //    await _parentsService.CreateParentAsync<T>(newParentInputModel);
        //    _parentsRepositoryMock.Object.All().Count().Should().Be(1);
        //}

        [Test]
        public async Task GetStudentIdsByParentUniqueIdTest()
        {
            OneTimeSetUp();
            await _parentsRepositoryMock.Object.AddAsync(NewParentCreate());
            var parent = _parentsRepositoryMock.Object.All().FirstOrDefault(p => p.UniqueId == ParentUniqueId);
            var actual = _parentsService.GetStudentIdsByParentUniqueId(ParentUniqueId);
            int expected = 1;
            expected.Should().Be(actual.Count);
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
