namespace Gradebook.Web.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Areas.Principal.ViewModels.InputModels;
    using ViewModels.InputModels;

    public interface IStudentsService
    {
        Task<T> CreateStudent<T>(StudentInputModel inputModel);

        IEnumerable<T> GetAllBySchoolIds<T>(IEnumerable<int> schoolIds);
        
        int? GetIdByUniqueId(string uniqueId);

        T GetById<T>(int id);
        
        Task EditAsync(StudentModifyInputModel modifiedModel);

        Task DeleteAsync(int id);
    }
}