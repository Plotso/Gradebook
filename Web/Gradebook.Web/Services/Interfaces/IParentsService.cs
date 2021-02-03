namespace Gradebook.Web.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Areas.Principal.ViewModels.InputModels;
    using ViewModels.InputModels;

    public interface IParentsService
    {
        Task<T> CreateParentAsync<T>(ParentInputModel inputModel);

        List<int> GetStudentIdsByParentUniqueId(string uniqueId);
        
        T GetById<T>(int id);
        
        Task EditAsync(ParentModifyInputModel modifiedModel);

        Task DeleteAsync(int id);
    }
}