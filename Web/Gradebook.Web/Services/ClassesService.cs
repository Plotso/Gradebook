namespace Gradebook.Web.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Data.Common.Repositories;
    using Data.Models;
    using Gradebook.Services.Mapping;
    using Interfaces;

    public class ClassesService : IClassesService
    {
        private readonly IDeletableEntityRepository<Class> _classesRepository;

        public ClassesService(IDeletableEntityRepository<Class> classesRepository)
        {
            _classesRepository = classesRepository;
        }

        public IEnumerable<T> GetAllByTeacherId<T>(int teacherId)
        {
            var classes = _classesRepository.All().Where(c => c.TeacherId == teacherId);
            return classes.To<T>();
        }

        public IEnumerable<T> GetAllByStudentId<T>(int studentId)
        {
            var classes = _classesRepository.All().Where(c => c.Students.Any(s => s.Id == studentId));
            return classes.To<T>();
        }

        public IEnumerable<T> GetAllByMultipleStudentIds<T>(List<int> studentIds)
        {
            var classes = _classesRepository.All().Where(c => c.Students.Any(s => studentIds.Contains(s.Id)));
            return classes.To<T>();
        }

        public IEnumerable<T> GetAllBySchoolId<T>(int schoolId)
        {
            var classes = _classesRepository.All().Where(c => c.Teacher.SchoolId == schoolId);
            return classes.To<T>();
        }

        public IEnumerable<T> GetAll<T>()
        {
            var classes = _classesRepository.All();
            return classes.To<T>();
        }

        public T GetById<T>(int id)
        {
            var classes = _classesRepository.All().Where(c => c.Id == id);
            return classes.To<T>().FirstOrDefault();
        }
    }
}