namespace Gradebook.Web.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Areas.Principal.ViewModels.InputModels;
    using Data.Common.Models;
    using Gradebook.Data.Common.Repositories;
    using Data.Models;
    using Gradebook.Services.Data.Interfaces;
    using Gradebook.Services.Mapping;
    using Interfaces;
    using ViewModels.InputModels;

    public class ParentsService : IParentsService
    {
        private readonly IDeletableEntityRepository<Parent> _parentsRepository;
        private readonly IRepository<Student> _studentsRepository;
        private readonly IDeletableEntityRepository<StudentParent> _studentParentsMappingRepository;
        private readonly IIdGeneratorService _idGeneratorService;

        public ParentsService(
            IDeletableEntityRepository<Parent> parentsRepository,
            IRepository<Student> studentsRepository, 
            IDeletableEntityRepository<StudentParent> studentParentsMappingRepository, 
            IIdGeneratorService idGeneratorService)
        {
            _parentsRepository = parentsRepository;
            _studentsRepository = studentsRepository;
            _studentParentsMappingRepository = studentParentsMappingRepository;
            _idGeneratorService = idGeneratorService;
        }

        public async Task<T> CreateParentAsync<T>(ParentInputModel inputModel)
        {
            var studentIds = inputModel.StudentIds.Select(int.Parse);
            var students = _studentsRepository.All().Where(s => studentIds.Contains(s.Id));
            if (students.Any())
            {
                var parent = new Parent
                {
                    FirstName = inputModel.FirstName,
                    LastName = inputModel.LastName,
                    PhoneNumber = inputModel.PhoneNumber,
                    UniqueId = _idGeneratorService.GenerateParentId()
                };

                await _parentsRepository.AddAsync(parent);
                await _parentsRepository.SaveChangesAsync();

                foreach (var student in students)
                {
                    var studentParentPair = new StudentParent
                    {
                        Student = student,
                        Parent = parent
                    };

                    await _studentParentsMappingRepository.AddAsync(studentParentPair);
                }

                await _studentParentsMappingRepository.SaveChangesAsync();

                BasePersonModel baseModel = _parentsRepository.All().FirstOrDefault(p => p.UniqueId == parent.UniqueId);

                return AutoMapperConfig.MapperInstance.Map<T>(baseModel);
            }

            throw new ArgumentException($"Sorry, we couldn't any student with id in the following list:  [{string.Join(", ", studentIds)}]");
        }

        public List<int> GetStudentIdsByParentUniqueId(string uniqueId)
        {
            var parent = _parentsRepository.All().FirstOrDefault(p => p.UniqueId == uniqueId);
            if (parent != null)
            {
                return parent.StudentParents.Select(s => s.StudentId).ToList();
            }

            return new List<int>();
        }
    }
}