namespace Gradebook.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using Gradebook.Data.Common.Repositories;
    using Gradebook.Data.Models;
    using Interfaces;
    using Mapping;

    public class SchoolsService : ISchoolsServices
    {
        private readonly IDeletableEntityRepository<School> _schoolsRepository;
        private readonly IUsersService _usersService;

        public SchoolsService(IDeletableEntityRepository<School> schoolsRepository, IUsersService usersService)
        {
            _schoolsRepository = schoolsRepository;
            _usersService = usersService;
        }

        public IEnumerable<T> GetAll<T>()
        {
            var schoolsQuery = _schoolsRepository.All();

            return schoolsQuery.To<T>().ToList();
        }

        public IEnumerable<T> GetAllByUserId<T>(string uniqueId)
        {
            var schools = _usersService.GetUserSchoolsByUniqueId(uniqueId);

            return schools.Select(s => AutoMapperConfig.MapperInstance.Map<T>(s));
        }
    }
}