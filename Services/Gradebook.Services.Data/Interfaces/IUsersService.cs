namespace Gradebook.Services.Data.Interfaces
{
    using Gradebook.Data.Common.Models;

    public interface IUsersService
    {
        UserType GetUserTypeByUniqueId(string uniqueId);
    }
}