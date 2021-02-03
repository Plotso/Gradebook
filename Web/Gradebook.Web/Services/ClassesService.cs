namespace Gradebook.Web.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Areas.Principal.ViewModels.InputModels;
    using Data.Common.Repositories;
    using Data.Models;
    using Gradebook.Services.Mapping;
    using Interfaces;
    using Microsoft.EntityFrameworkCore.Internal;
    using ViewModels.Classes;
    using ViewModels.Home;

    public class ClassesService : IClassesService
    {
        private readonly IDeletableEntityRepository<Class> _classesRepository;
        private readonly IDeletableEntityRepository<Student> _studentRepository;
        private readonly IRepository<Teacher> _teachersRepository;
        private readonly IRepository<School> _schoolsRepository;

        public ClassesService(IDeletableEntityRepository<Class> classesRepository, IDeletableEntityRepository<Student> studentRepository, IRepository<Teacher> teachersRepository, IRepository<School> schoolsRepository)
        {
            _classesRepository = classesRepository;
            _studentRepository = studentRepository;
            _teachersRepository = teachersRepository;
            _schoolsRepository = schoolsRepository;
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

        public IEnumerable<T> GetAllByMultipleSchoolIds<T>(List<int> schoolIds)
        {
            var classes = _classesRepository.All().Where(c => schoolIds.Contains(c.Teacher.SchoolId));
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

        public async Task CreateAsync(ClassInputModel inputModel)
        {
            var teacherId = int.Parse(inputModel.TeacherId);
            var teacher = _teachersRepository.All().FirstOrDefault(t => t.Id == teacherId);
            if (teacher != null)
            {
                if (teacher.Class != null)
                {
                    throw new ArgumentException($"Sorry, teacher with id {teacherId} is already registered as head of another class");
                }

                var isClassLetterNumberCombinationAlreadyOccupied = _classesRepository.All().Any(c =>
                    c.Letter == inputModel.Letter &&
                    c.YearCreated == inputModel.YearCreated &&
                    c.Year == inputModel.Year &&
                    c.Teacher.SchoolId == teacher.SchoolId);
                if (isClassLetterNumberCombinationAlreadyOccupied)
                {
                    throw new ArgumentException($"Sorry, there is already existing class for year {inputModel.YearCreated} that's currently in {inputModel.Year} grade and with letter {inputModel.Letter}");
                }

                var schoolClass = new Class
                {
                    Letter = inputModel.Letter,
                    Year = inputModel.Year,
                    YearCreated = inputModel.YearCreated,
                    Teacher = teacher
                };

                await _classesRepository.AddAsync(schoolClass);
                await _classesRepository.SaveChangesAsync();

                var classEntity = _classesRepository.All().First(c =>
                    c.Letter == schoolClass.Letter && c.YearCreated == schoolClass.YearCreated &&
                    c.Year == schoolClass.Year);
                var school = teacher.School;
                school.Classes.Add(classEntity);
                
                _schoolsRepository.Update(school);
                await _schoolsRepository.SaveChangesAsync();

                return;
            }

            throw new ArgumentException($"Sorry, we couldn't find teacher with id {teacherId}");
        }

        public async Task EditAsync(ClassModifyInputModel modifiedModel)
        {
            var schoolClass = _classesRepository.All().FirstOrDefault(c => c.Id == modifiedModel.Id);
            if (schoolClass != null)
            {
                var inputModel = modifiedModel.Class;
                var teacherId = int.Parse(inputModel.TeacherId);

                var isClassLetterNumberCombinationAlreadyOccupied = _classesRepository.All().Any(c =>
                    c.Letter == inputModel.Letter &&
                    c.YearCreated == inputModel.YearCreated &&
                    c.Year == inputModel.Year &&
                    c.TeacherId == teacherId);
                if (isClassLetterNumberCombinationAlreadyOccupied)
                {
                    throw new ArgumentException($"Sorry, there is already existing class for year {inputModel.YearCreated} that's currently in {inputModel.Year} grade and with letter {inputModel.Letter}");
                }

                schoolClass.Letter = inputModel.Letter;
                schoolClass.Year = inputModel.Year;
                schoolClass.YearCreated = inputModel.YearCreated;

                if (schoolClass.TeacherId != teacherId)
                {
                    var teacher = _teachersRepository.All().FirstOrDefault(t => t.Id == teacherId);
                    if (teacher != null)
                    {
                        if (teacher.Class != null)
                        {
                            throw new ArgumentException($"Sorry, teacher with id {teacherId} is already registered as head of another class");
                        }

                        if (teacher.SchoolId != schoolClass.Teacher.SchoolId)
                        {
                            var school = teacher.School;
                            school.Classes.Add(schoolClass);

                            _schoolsRepository.Update(school);
                            await _schoolsRepository.SaveChangesAsync();
                        }

                        // MUST be below the if above
                        schoolClass.Teacher = teacher;
                    }
                }

                _classesRepository.Update(schoolClass);
                await _classesRepository.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var schoolClass = _classesRepository.All().FirstOrDefault(c => c.Id == id);
            if (schoolClass != null)
            {
                var students = schoolClass.Students;
                foreach (var student in students)
                {
                    _studentRepository.Delete(student);
                }

                await _studentRepository.SaveChangesAsync();

                schoolClass.Teacher = null;
                schoolClass.TeacherId = null;
                _classesRepository.Update(schoolClass);
                _classesRepository.Delete(schoolClass);
                await _classesRepository.SaveChangesAsync();
                
                await _teachersRepository.SaveChangesAsync();
            }
        }
    }
}