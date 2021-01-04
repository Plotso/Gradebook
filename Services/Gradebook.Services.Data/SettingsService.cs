namespace Gradebook.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using Gradebook.Data.Common.Repositories;
    using Gradebook.Data.Models;
    using Interfaces;
    using Mapping;

    public class SettingsService : ISettingsService
    {
        private readonly IDeletableEntityRepository<Setting> settingsRepository;

        public SettingsService(IDeletableEntityRepository<Setting> settingsRepository)
        {
            this.settingsRepository = settingsRepository;
        }

        public int GetCount()
        {
            return settingsRepository.AllAsNoTracking().Count();
        }

        public IEnumerable<T> GetAll<T>()
        {
            return settingsRepository.All().To<T>().ToList();
        }
    }
}
