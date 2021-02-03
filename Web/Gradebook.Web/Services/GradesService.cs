namespace Gradebook.Web.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Areas.Teacher.ViewModels.InputModels;
    using Data.Common.Repositories;
    using Data.Models;
    using Data.Models.Grades;
    using Gradebook.Services.Mapping;
    using Interfaces;
    using ViewModels.InputModels;

    public class GradesService : IGradesService
    {
        private readonly IDeletableEntityRepository<Grade> _gradesRepository;
        private readonly IRepository<Teacher> _teachersRepository;
        private readonly IRepository<StudentSubject> _studentSubjectsRepository;

        public GradesService(IDeletableEntityRepository<Grade> gradesRepository, IRepository<Teacher> teachersRepository, IRepository<StudentSubject> studentSubjectsRepository)
        {
            _gradesRepository = gradesRepository;
            _teachersRepository = teachersRepository;
            _studentSubjectsRepository = studentSubjectsRepository;
        }

        public T GetById<T>(int id)
        {
            var grades = _gradesRepository.All().Where(g => g.Id == id);
            return grades.To<T>().FirstOrDefault();
        }

        public async Task CreateAsync(GradeInputModel inputModel)
        {
            var studentSubject = _studentSubjectsRepository.All().FirstOrDefault(s =>
                s.StudentId == inputModel.StudentId && s.SubjectId == inputModel.SubjectId);
            if (studentSubject != null)
            {
                var teacher = _teachersRepository.All().FirstOrDefault(t => t.Id == inputModel.TeacherId);
                if (teacher != null)
                {
                    var grade = new Grade
                    {
                        Value = inputModel.Value,
                        Period = inputModel.Period,
                        Type = inputModel.Type,
                        StudentSubject = studentSubject,
                        Teacher = teacher
                    };

                    await _gradesRepository.AddAsync(grade);
                    await _gradesRepository.SaveChangesAsync();

                    return;
                }

                throw new ArgumentException($"Sorry, we couldn't find teacher with id {inputModel.TeacherId}");
            }

            throw new ArgumentException($"Sorry, we couldn't find pair of student ({inputModel.StudentId}) and subject({inputModel.SubjectId})");
        }

        public async Task EditAsync(GradeModifyInputModel modifiedModel)
        {
            var grade = _gradesRepository.All().FirstOrDefault(g => g.Id == modifiedModel.Id);
            if (grade != null)
            {
                var inputModel = modifiedModel.Grade;
                grade.Value = inputModel.Value;
                grade.Period = inputModel.Period;
                grade.Type = inputModel.Type;

                _gradesRepository.Update(grade);
                await _gradesRepository.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var grade = _gradesRepository.All().FirstOrDefault(g => g.Id == id);
            if (grade != null)
            {
                _gradesRepository.Delete(grade);
                await _gradesRepository.SaveChangesAsync();
            }
        }
    }
}