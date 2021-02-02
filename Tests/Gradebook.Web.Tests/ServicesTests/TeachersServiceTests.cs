namespace Gradebook.Web.Tests.ServicesTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Areas.Administration.ViewModels.InputModels;
    using Data.Common.Repositories;
    using Data.Models;
    using FluentAssertions;
    using Gradebook.Services.Data;
    using Gradebook.Services.Data.Interfaces;
    using Gradebook.Services.Mapping;
    using Moq;
    using NUnit.Framework;
    using Services;
    using Services.Interfaces;
    using ViewModels;
    using ViewModels.Principal;

    [TestFixture]
    public class TeachersServiceTests
    {
        private const int TestTeacherDBId = 2;
        private const string TestTeacherUniqueID = "T021test";
        
        private Mock<IDeletableEntityRepository<Teacher>> _teachersRepositoryMock;
        private Mock<IDeletableEntityRepository<School>> _schoolsRepositoryMock;
        private ITeachersService _teachersService;

        [SetUp]
        public void Setup()
        {
            _teachersRepositoryMock = new Mock<IDeletableEntityRepository<Teacher>>();
            _schoolsRepositoryMock = new Mock<IDeletableEntityRepository<School>>();
            _teachersService = new TeachersService(
                _teachersRepositoryMock.Object,
                _schoolsRepositoryMock.Object,
                new IdGeneratorService());
        }

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly, typeof(SchoolInputModel).GetTypeInfo().Assembly);
        }

        [Test]
        public void GetIdByUniqueId_WithExistingTeacher_ShouldReturnCorrectId()
        {
            _teachersRepositoryMock.Setup(t => t.All())
                .Returns(new List<Teacher> {new Teacher {Id = TestTeacherDBId, UniqueId = TestTeacherUniqueID}}.AsQueryable());

            var teacherId = _teachersService.GetIdByUniqueId(TestTeacherUniqueID);
            
            teacherId.Should().Be(TestTeacherDBId);
        }

        [Test]
        public void GetIdByUniqueId_WithMissingTeacher_ShouldReturnNull()
        {
            _teachersRepositoryMock.Setup(t => t.All())
                .Returns(new List<Teacher> {new Teacher {Id = TestTeacherDBId, UniqueId = TestTeacherUniqueID}}.AsQueryable());

            var teacherId = _teachersService.GetIdByUniqueId($"{TestTeacherUniqueID}0");
            
            teacherId.Should().BeNull();
        }
    }
}