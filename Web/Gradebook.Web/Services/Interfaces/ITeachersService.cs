namespace Gradebook.Web.Services.Interfaces
{
    using System.Threading.Tasks;
    using Areas.Principal.ViewModels.InputModels;
    using ViewModels.InputModels;

    public interface ITeachersService
    {
        int? GetIdByUniqueId(string uniqueId);
        
        T GetById<T>(int id);

        Task<T> CreateTeacher<T>(TeacherInputModel inputModel);

        Task EditAsync(TeacherModifyInputModel modifiedModel);

        Task DeleteAsync(int id);
    }
}
