namespace Gradebook.Web.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Data.Common.Models;
    using Data.Models;
    using ViewModels.InputModels;

    public interface IUsersService
    {
        UserType GetUserTypeByUniqueId(string uniqueId);

        IEnumerable<School> GetUserSchoolsByUniqueId(string uniqueId);

        Task<T> CreatePrincipal<T>(PrincipalInputModel inputModel);

        Principal GetPrincipalByUniqueId(string uniqueId);

        Task SetUserEmail(string uniqueId, string email);
    }
}