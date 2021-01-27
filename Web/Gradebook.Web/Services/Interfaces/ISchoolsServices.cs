namespace Gradebook.Web.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Areas.Administration.ViewModels.InputModels;

    public interface ISchoolsServices
    {
        T GetById<T>(int id);

        Task DeleteAsync(int id);

        Task EditAsync(SchoolModifyInputModel modifiedModel);

        IEnumerable<T> GetAll<T>();

        IEnumerable<T> GetAllByUserId<T>(string uniqueId, bool isAdmin = false);

        Task Create(SchoolInputModel inputModel, string principalUniqueId);
    }
}