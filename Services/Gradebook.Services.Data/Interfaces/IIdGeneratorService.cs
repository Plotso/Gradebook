namespace Gradebook.Services.Data.Interfaces
{
    public interface IIdGeneratorService
    {
        string GeneratePrincipalId();

        string GenerateTeacherId();

        string GenerateParentId();

        string GenerateStudentId();
    }
}