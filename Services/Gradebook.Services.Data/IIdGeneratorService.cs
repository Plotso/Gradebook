namespace Gradebook.Services.Data
{
    public interface IIdGeneratorService
    {
        string GeneratePrincipalId();

        string GenerateTeacherId();

        string GenerateParentId();

        string GenerateStudentId();
    }
}