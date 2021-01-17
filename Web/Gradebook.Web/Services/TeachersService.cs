namespace Gradebook.Web.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Areas.Principal.ViewModels.InputModels;
    using Data.Common.Models;
    using Data.Common.Repositories;
    using Data.Models;
    using Gradebook.Services.Data.Interfaces;
    using Gradebook.Services.Mapping;
    using Interfaces;
    using ViewModels.InputModels;

    public class TeachersService : ITeachersService
    {
        private readonly IDeletableEntityRepository<Teacher> _teachersRepository;
        private readonly IDeletableEntityRepository<School> _schoolsRepository;
        private readonly IIdGeneratorService _idGeneratorService;

        public TeachersService(
            IDeletableEntityRepository<Teacher> teachersRepository,
            IDeletableEntityRepository<School> schoolsRepository,
            IIdGeneratorService idGeneratorService)
        {
            _teachersRepository = teachersRepository;
            _schoolsRepository = schoolsRepository;
            _idGeneratorService = idGeneratorService;
        }

        public int? GetIdByUniqueId(string uniqueId)
        {
            var teacher = _teachersRepository.All().FirstOrDefault(t => t.UniqueId == uniqueId);
            return teacher?.Id;
        }

        public T GetById<T>(int id)
        {
            var teacher = _teachersRepository.All().Where(t => t.Id == id);
            return teacher.To<T>().FirstOrDefault();
        }

        public async Task<T> CreateTeacher<T>(TeacherInputModel inputModel)
        {
            var schoolId = int.Parse(inputModel.SchoolId);
            var school = _schoolsRepository.All().FirstOrDefault(s => s.Id == schoolId);
            if (school != null)
            {
                    var teacher = new Teacher()
                    {
                        FirstName = inputModel.FirstName,
                        LastName = inputModel.LastName,
                        School = school,
                        UniqueId = _idGeneratorService.GenerateTeacherId()
                    };

                    await _teachersRepository.AddAsync(teacher);
                    await _teachersRepository.SaveChangesAsync();

                    BasePersonModel baseModel = _teachersRepository.All().FirstOrDefault(t => t.UniqueId == teacher.UniqueId);

                    return AutoMapperConfig.MapperInstance.Map<T>(baseModel);
            }

            throw new ArgumentException($"Sorry, we couldn't find school with id {schoolId}");
        }

        public async Task EditAsync(TeacherModifyInputModel modifiedModel)
        {
            var teacher = _teachersRepository.All().FirstOrDefault(s => s.Id == modifiedModel.Id);
            if (teacher != null)
            {
                var inputModel = modifiedModel.Teacher;

                teacher.FirstName = inputModel.FirstName;
                teacher.LastName = inputModel.LastName;

                var schoolId = int.Parse(inputModel.SchoolId);
                var school = _schoolsRepository.All().FirstOrDefault(s => s.Id == schoolId);
                if (school != null)
                {
                    teacher.School = school;
                }

                _teachersRepository.Update(teacher);
                await _teachersRepository.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var teacher = _teachersRepository.All().FirstOrDefault(s => s.Id == id);
            if (teacher != null)
            {
                _teachersRepository.Delete(teacher);
                await _teachersRepository.SaveChangesAsync();
            }
        }
    }
}