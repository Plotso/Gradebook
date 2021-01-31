namespace Gradebook.Web.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Areas.Principal.ViewModels.InputModels;
    using ViewModels.Classes;

    public interface IClassesService
    {
        IEnumerable<T> GetAllByTeacherId<T>(int teacherId);

        IEnumerable<T> GetAllByStudentId<T>(int studentId);

        IEnumerable<T> GetAllByMultipleStudentIds<T>(List<int> studentIds);

        IEnumerable<T> GetAllBySchoolId<T>(int schoolId);

        IEnumerable<T> GetAll<T>();

        T GetById<T>(int id);
        
        Task CreateAsync(ClassInputModel inputModel);

        Task EditAsync(ClassModifyInputModel modifiedModel);

        Task DeleteAsync(int id);
    }
}