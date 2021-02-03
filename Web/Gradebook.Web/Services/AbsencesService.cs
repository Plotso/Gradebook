namespace Gradebook.Web.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Areas.Teacher.ViewModels.InputModels;
    using Data.Common.Repositories;
    using Data.Models;
    using Data.Models.Absences;
    using Data.Models.Grades;
    using Gradebook.Services.Mapping;
    using Interfaces;
    using ViewModels.InputModels;

    public class AbsencesService : IAbsencesService
    {
        private readonly IDeletableEntityRepository<Absence> _absencesRepository;
        private readonly IRepository<Teacher> _teachersRepository;
        private readonly IRepository<StudentSubject> _studentSubjectsRepository;

        public AbsencesService(IDeletableEntityRepository<Absence> absencesRepository, IRepository<Teacher> teachersRepository, IRepository<StudentSubject> studentSubjectsRepository)
        {
            _absencesRepository = absencesRepository;
            _teachersRepository = teachersRepository;
            _studentSubjectsRepository = studentSubjectsRepository;
        }

        public T GetById<T>(int id)
        {
            var absences = _absencesRepository.All().Where(a => a.Id == id);
            return absences.To<T>().FirstOrDefault();
        }

        public async Task CreateAsync(AbsenceInputModel inputModel)
        {
            var studentSubject = _studentSubjectsRepository.All().FirstOrDefault(s =>
                s.StudentId == inputModel.StudentId && s.SubjectId == inputModel.SubjectId);
            if (studentSubject != null)
            {
                var teacher = _teachersRepository.All().FirstOrDefault(t => t.Id == inputModel.TeacherId);
                if (teacher != null)
                {
                    var grade = new Absence
                    {
                        Period = inputModel.Period,
                        Type = inputModel.Type,
                        StudentSubject = studentSubject,
                        Teacher = teacher
                    };

                    await _absencesRepository.AddAsync(grade);
                    await _absencesRepository.SaveChangesAsync();

                    return;
                }

                throw new ArgumentException($"Sorry, we couldn't find teacher with id {inputModel.TeacherId}");
            }

            throw new ArgumentException($"Sorry, we couldn't find pair of student ({inputModel.StudentId}) and subject({inputModel.SubjectId})");
        }

        public async Task EditAsync(AbsenceModifyInputModel modifiedModel)
        {
            var absence = _absencesRepository.All().FirstOrDefault(a => a.Id == modifiedModel.Id);
            if (absence != null)
            {
                var inputModel = modifiedModel.Absence;
                absence.Period = inputModel.Period;
                absence.Type = inputModel.Type;

                _absencesRepository.Update(absence);
                await _absencesRepository.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var absence = _absencesRepository.All().FirstOrDefault(a => a.Id == id);
            if (absence != null)
            {
                _absencesRepository.Delete(absence);
                await _absencesRepository.SaveChangesAsync();
            }
        }
    }
}