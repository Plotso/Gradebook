namespace Gradebook.Web.Services.Interfaces
{
    using System.Threading.Tasks;

    using ViewModels.InputModels;

    public interface ITeachersService
    {
        Task<T> CreateStudent<T>(TeacherInputModel inputModel);
    }
}
