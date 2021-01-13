namespace Gradebook.Web.Services.Interfaces
{
    using System.Threading.Tasks;
    using ViewModels.InputModels;

    public interface IStudentsService
    {
        Task<T> CreateStudent<T>(StudentInputModel inputModel);
    }
}