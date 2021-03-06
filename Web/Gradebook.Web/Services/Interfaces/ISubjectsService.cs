﻿namespace Gradebook.Web.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Areas.Principal.ViewModels.InputModels;
    using ViewModels.Principal;
    using ViewModels.Subject;

    public interface ISubjectsService
    {
        IEnumerable<T> GetAllByTeacherId<T>(int teacherId);

        IEnumerable<T> GetAllByStudentId<T>(int studentId);

        IEnumerable<T> GetAllByMultipleStudentIds<T>(List<int> studentIds);

        IEnumerable<T> GetAllBySchoolId<T>(int schoolId);

        IEnumerable<T> GetAllByMultipleSchoolIds<T>(List<int> schoolIds);

        IEnumerable<T> GetAll<T>();

        T GetById<T>(int id);

        Task CreateSubject(SubjectInputModel inputModel);

        Task EditAsync(SubjectModifyInputModel modifiedModel);

        Task DeleteAsync(int id);

        T GetStudentSubjectPair<T>(int studentId, int subjectId);

        Task AssignAllStudentsFromClassToSubject(ClassSubjectInputModel inputModel);
    }
}