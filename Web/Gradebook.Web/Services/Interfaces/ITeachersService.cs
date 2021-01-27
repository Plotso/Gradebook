namespace Gradebook.Web.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Areas.Principal.ViewModels.InputModels;
    using ViewModels.InputModels;

    public interface ITeachersService
    {
        int? GetIdByUniqueId(string uniqueId);

        T GetById<T>(int id);

        IEnumerable<T> GetAllBySchoolIds<T>(IEnumerable<int> schoolIds);

        Task<T> CreateTeacher<T>(TeacherInputModel inputModel);

        Task EditAsync(TeacherModifyInputModel modifiedModel);

        Task DeleteAsync(int id);
    }
}
