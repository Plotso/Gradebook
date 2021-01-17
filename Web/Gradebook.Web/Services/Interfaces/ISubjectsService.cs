namespace Gradebook.Web.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ISubjectsService
    {
        IEnumerable<T> GetAllByTeacherId<T>(int teacherId);

        IEnumerable<T> GetAllByStudentId<T>(int studentId);

        IEnumerable<T> GetAllBySchoolId<T>(int schoolId);

        IEnumerable<T> GetAll<T>();

        T GetById<T>(int id);

        Task DeleteAsync(int id);
    }
}