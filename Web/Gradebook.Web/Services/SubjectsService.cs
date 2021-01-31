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
    using ViewModels.Subject;

    public class SubjectsService : ISubjectsService
    {
        private readonly IDeletableEntityRepository<Subject> _subjectsRepository;
        private readonly IDeletableEntityRepository<StudentSubject> _studentSubjectRepository;
        private readonly IDeletableEntityRepository<Teacher> _teachersRepository;

        public SubjectsService(IDeletableEntityRepository<Subject> subjectsRepository, IDeletableEntityRepository<StudentSubject> studentSubjectRepository, IDeletableEntityRepository<Teacher> teachersRepository)
        {
            _subjectsRepository = subjectsRepository;
            _studentSubjectRepository = studentSubjectRepository;
            _teachersRepository = teachersRepository;
        }

        public IEnumerable<T> GetAllByTeacherId<T>(int teacherId)
        {
            var subjects = _subjectsRepository.All().Where(s => s.TeacherId == teacherId);
            return subjects.To<T>().ToList();
        }

        public IEnumerable<T> GetAllByStudentId<T>(int studentId)
        {
            var studentSubjects = _studentSubjectRepository.All().Where(s => s.StudentId == studentId);
            var subjects = studentSubjects.Select(s => s.Subject);
            return subjects.To<T>().ToList();
        }

        public IEnumerable<T> GetAllByMultipleStudentIds<T>(List<int> studentIds)
        {
            var studentSubjects = _studentSubjectRepository.All().Where(s => studentIds.Contains(s.StudentId));
            var subjects = studentSubjects.Select(s => s.Subject);
            return subjects.To<T>().ToList();
        }

        public IEnumerable<T> GetAllBySchoolId<T>(int schoolId)
        {
            var subjects = _subjectsRepository.All().Where(s => s.Teacher.SchoolId == schoolId);
            return subjects.To<T>().ToList();
        }

        public IEnumerable<T> GetAll<T>()
        {
            var subjects = _subjectsRepository.All();
            return subjects.To<T>().ToList();
        }

        public T GetById<T>(int id)
        {
            var subject = _subjectsRepository.All().Where(s => s.Id == id);
            return subject.To<T>().FirstOrDefault();
        }

        public async Task CreateSubject(SubjectInputModel inputModel)
        {
            var teacherId = int.Parse(inputModel.TeacherId);
            var teacher = _teachersRepository.All().FirstOrDefault(t => t.Id == teacherId);
            if (teacher != null)
            {
                var subject = new Subject
                {
                    Name = inputModel.Name,
                    YearGrade = inputModel.YearGrade,
                    SchoolYear = inputModel.SchoolYear,
                    Teacher = teacher
                };

                await _subjectsRepository.AddAsync(subject);
                await _subjectsRepository.SaveChangesAsync();

                return;
            }

            throw new ArgumentException($"Sorry, we couldn't find teacher with id {teacherId}");
        }

        public async Task EditAsync(SubjectModifyInputModel modifiedModel)
        {
            var subject = _subjectsRepository.All().FirstOrDefault(s => s.Id == modifiedModel.Id);
            if (subject != null)
            {
                var inputModel = modifiedModel.Subject;
                subject.Name = inputModel.Name;
                subject.YearGrade = inputModel.YearGrade;
                subject.SchoolYear = inputModel.SchoolYear;

                var teacherId = int.Parse(inputModel.TeacherId);
                if (subject.TeacherId != teacherId)
                {
                    var teacher = _teachersRepository.All().FirstOrDefault(t => t.Id == teacherId);
                    if (teacher != null)
                    {
                        subject.Teacher = teacher;
                    }
                }

                _subjectsRepository.Update(subject);
                await _subjectsRepository.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var subject = _subjectsRepository.All().FirstOrDefault(s => s.Id == id);
            if (subject != null)
            {
                if (subject.StudentSubjects.Any())
                {
                    var allStudentsForSubject = _studentSubjectRepository.All().Where(s => s.SubjectId == id);
                    foreach (var studentSubjectPair in allStudentsForSubject)
                    {
                        _studentSubjectRepository.Delete(studentSubjectPair);  // ToDo: Decide if grades/absences delete behaviour should be handled here as well
                    }

                    await _studentSubjectRepository.SaveChangesAsync();
                }

                _subjectsRepository.Delete(subject);
                await _subjectsRepository.SaveChangesAsync();
            }
        }
    }
}