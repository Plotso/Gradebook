namespace Gradebook.Services.Data.Tests
{
    using Common;
    using FluentAssertions;
    using Interfaces;
    using NUnit.Framework;

    [TestFixture]
    public class IDGeneratorServiceTests
    {
        private IIdGeneratorService _idGeneratorService;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _idGeneratorService = new IdGeneratorService();
        }

        [Test]
        public void GeneratePrincipalId_WithSingleId_ShouldGenerateIdForPrincipal()
        {
            var id = _idGeneratorService.GeneratePrincipalId();

            id.Should().StartWith(GlobalConstants.PrincipalIdPrefix.ToString());
        }

        [Test]
        public void GeneratePrincipalId_WithMultipleIds_ShouldGenerateUniqueIdsForPrincipals()
        {
            var firstId = _idGeneratorService.GeneratePrincipalId();
            var secondId = _idGeneratorService.GeneratePrincipalId();

            firstId.Should().StartWith(GlobalConstants.PrincipalIdPrefix.ToString());
            secondId.Should().StartWith(GlobalConstants.PrincipalIdPrefix.ToString());
            secondId.Should().NotBe(firstId);
        }

        [Test]
        public void GenerateTeacherId_WithSingleId_ShouldGenerateIdForTeacher()
        {
            var id = _idGeneratorService.GenerateTeacherId();

            id.Should().StartWith(GlobalConstants.TeacherIdPrefix.ToString());
        }

        [Test]
        public void GenerateTeacherId_WithMultipleIds_ShouldGenerateUniqueIdsForTeachers()
        {
            var firstId = _idGeneratorService.GenerateTeacherId();
            var secondId = _idGeneratorService.GenerateTeacherId();

            firstId.Should().StartWith(GlobalConstants.TeacherIdPrefix.ToString());
            secondId.Should().StartWith(GlobalConstants.TeacherIdPrefix.ToString());
            secondId.Should().NotBe(firstId);
        }

        [Test]
        public void GenerateStudentId_WithSingleId_ShouldGenerateIdForStudent()
        {
            var id = _idGeneratorService.GenerateStudentId();

            id.Should().StartWith(GlobalConstants.StudentIdPrefix.ToString());
        }

        [Test]
        public void GenerateStudentId_WithMultipleIds_ShouldGenerateUniqueIdsForStudents()
        {
            var firstId = _idGeneratorService.GenerateStudentId();
            var secondId = _idGeneratorService.GenerateStudentId();

            firstId.Should().StartWith(GlobalConstants.StudentIdPrefix.ToString());
            secondId.Should().StartWith(GlobalConstants.StudentIdPrefix.ToString());
            secondId.Should().NotBe(firstId);
        }

        [Test]
        public void GenerateParentId_WithSingleId_ShouldGenerateIdForParent()
        {
            var id = _idGeneratorService.GenerateParentId();

            id.Should().StartWith(GlobalConstants.ParentIdPrefix.ToString());
        }

        [Test]
        public void GenerateParentId_WithMultipleIds_ShouldGenerateUniqueIdsForParents()
        {
            var firstId = _idGeneratorService.GenerateParentId();
            var secondId = _idGeneratorService.GenerateParentId();

            firstId.Should().StartWith(GlobalConstants.ParentIdPrefix.ToString());
            secondId.Should().StartWith(GlobalConstants.ParentIdPrefix.ToString());
            secondId.Should().NotBe(firstId);
        }
    }
}