namespace Gradebook.Web.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Areas.Administration.ViewModels.InputModels;
    using Data.Common.Repositories;
    using Data.Models;
    using Gradebook.Services.Mapping;
    using Interfaces;

    public class SchoolsService : ISchoolsServices
    {
        private readonly IDeletableEntityRepository<School> _schoolsRepository;
        private readonly IUsersService _usersService;
        private readonly IFileManagementService _fileManagementService;

        public SchoolsService(IDeletableEntityRepository<School> schoolsRepository, IUsersService usersService, IFileManagementService fileManagementService)
        {
            _schoolsRepository = schoolsRepository;
            _usersService = usersService;
            _fileManagementService = fileManagementService;
        }

        public T GetById<T>(int id)
        {
            var school = _schoolsRepository.All().Where(s => s.Id == id);
            return school.To<T>().FirstOrDefault();
        }

        public async Task DeleteAsync(int id)
        {
            var school = _schoolsRepository.All().FirstOrDefault(s => s.Id == id);
            if (school != null)
            {
                var allPeopleConnectedToSchoolUniqueIds = new List<string>();
                allPeopleConnectedToSchoolUniqueIds.AddRange(school.Teachers.Select(t => t.UniqueId).ToList());
                allPeopleConnectedToSchoolUniqueIds.AddRange(school.Students.Select(s => s.UniqueId).ToList());
                allPeopleConnectedToSchoolUniqueIds.Add(school.Principal.UniqueId);

                foreach (var userUniqueId in allPeopleConnectedToSchoolUniqueIds)
                {
                    await _usersService.DeleteByUniqueIdAsync(userUniqueId);
                }

                // ToDo: Add logic for classes deletion, then the logic for students above would be obsolete
                _schoolsRepository.Delete(school);
                await _schoolsRepository.SaveChangesAsync();
            }
        }

        public async Task EditAsync(SchoolModifyInputModel modifiedModel)
        {
            var school = _schoolsRepository.All().FirstOrDefault(s => s.Id == modifiedModel.Id);
            if (school != null)
            {
                var modifiedSchool = modifiedModel.School;
                school.Name = modifiedSchool.Name;
                school.Address = modifiedSchool.Address;
                school.Type = modifiedSchool.Type;
                school.Name = modifiedSchool.Name;

                if (modifiedSchool.SchoolImage != null)
                {
                    var fileName = modifiedSchool.SchoolImage.Name;
                    var uniqueFileName = Guid.NewGuid() + fileName;

                    await _fileManagementService.SaveImageAsync("schools", uniqueFileName, modifiedSchool.SchoolImage);
                    school.SchoolImageName = uniqueFileName;
                }

                _schoolsRepository.Update(school);
                await _schoolsRepository.SaveChangesAsync();
            }
        }

        public IEnumerable<T> GetAll<T>()
        {
            var schoolsQuery = _schoolsRepository.All();

            return schoolsQuery.To<T>().ToList();
        }

        public IEnumerable<T> GetAllByUserId<T>(string uniqueId, bool isAdmin = false)
        {
            IEnumerable<School> schools;
            if (isAdmin)
            {
                schools = _schoolsRepository.All();
            }
            else
            {
                schools = _usersService.GetUserSchoolsByUniqueId(uniqueId);
            }

            return schools.Select(s => AutoMapperConfig.MapperInstance.Map<T>(s));
        }

        public async Task Create(SchoolInputModel inputModel, string principalUniqueId)
        {
            var principal = _usersService.GetPrincipalByUniqueId(principalUniqueId);
            var school = new School
            {
                Name = inputModel.Name,
                Address = inputModel.Address,
                Type = inputModel.Type,
                Principal = principal
            };

            if (inputModel.SchoolImage != null)
            {
                var fileName = inputModel.SchoolImage.Name;
                var uniqueFileName = Guid.NewGuid() + fileName;

                await _fileManagementService.SaveImageAsync("schools", uniqueFileName, inputModel.SchoolImage);
                school.SchoolImageName = uniqueFileName;
            }

            await _schoolsRepository.AddAsync(school);
            await _schoolsRepository.SaveChangesAsync();
        }
    }
}