namespace Gradebook.Web.Services.Interfaces
{
    using System.Collections.Generic;

    public interface IClassesService
    {
        IEnumerable<T> GetAllByTeacherId<T>(int teacherId);

        IEnumerable<T> GetAllByStudentId<T>(int studentId);

        IEnumerable<T> GetAllByMultipleStudentIds<T>(List<int> studentIds);

        IEnumerable<T> GetAllBySchoolId<T>(int schoolId);

        IEnumerable<T> GetAll<T>();

        T GetById<T>(int id);
    }
}