namespace Gradebook.Web.Services.Interfaces
{
    using System.Threading.Tasks;
    using Areas.Teacher.ViewModels.InputModels;
    using ViewModels.InputModels;

    public interface IAbsencesService
    {
        T GetById<T>(int id);

        Task CreateAsync(AbsenceInputModel inputModel);

        Task EditAsync(AbsenceModifyInputModel modifiedModel);

        Task DeleteAsync(int id);
    }
}