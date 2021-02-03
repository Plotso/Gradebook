namespace Gradebook.Web.Services.Interfaces
{
    using System.Threading.Tasks;
    using Areas.Teacher.ViewModels.InputModels;
    using ViewModels.InputModels;

    public interface IGradesService
    {
        T GetById<T>(int id);

        Task CreateAsync(GradeInputModel inputModel);

        Task EditAsync(GradeModifyInputModel modifiedModel);

        Task DeleteAsync(int id);
    }
}