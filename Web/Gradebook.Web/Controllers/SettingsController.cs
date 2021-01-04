﻿namespace Gradebook.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Data.Common.Repositories;
    using Data.Models;
    using Microsoft.AspNetCore.Mvc;
    using Services.Data;
    using ViewModels.Settings;

    public class SettingsController : BaseController
    {
        private readonly ISettingsService settingsService;

        private readonly IDeletableEntityRepository<Setting> repository;

        public SettingsController(ISettingsService settingsService, IDeletableEntityRepository<Setting> repository)
        {
            this.settingsService = settingsService;
            this.repository = repository;
        }

        public IActionResult Index()
        {
            var settings = settingsService.GetAll<SettingViewModel>();
            var model = new SettingsListViewModel { Settings = settings };
            return View(model);
        }

        public async Task<IActionResult> InsertSetting()
        {
            var random = new Random();
            var setting = new Setting { Name = $"Name_{random.Next()}", Value = $"Value_{random.Next()}" };

            await repository.AddAsync(setting);
            await repository.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
