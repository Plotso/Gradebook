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