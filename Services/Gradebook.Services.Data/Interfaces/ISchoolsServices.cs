namespace Gradebook.Services.Data.Interfaces
{
    using System.Collections.Generic;

    public interface ISchoolsServices
    {
        IEnumerable<T> GetAll<T>();

        IEnumerable<T> GetAllByUserId<T>(string uniqueId);
    }
}