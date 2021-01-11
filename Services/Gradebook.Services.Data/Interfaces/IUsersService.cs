namespace Gradebook.Services.Data.Interfaces
{
    using System.Collections.Generic;
    using Gradebook.Data.Common.Models;
    using Gradebook.Data.Models;

    public interface IUsersService
    {
        UserType GetUserTypeByUniqueId(string uniqueId);

        IEnumerable<School> GetUserSchoolsByUniqueId(string uniqueId);
    }
}