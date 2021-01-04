namespace Gradebook.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Services.Data;
    using Services.Data.Interfaces;
    using ViewModels.Administration.Dashboard;

    public class DashboardController : AdministrationController
    {
        private readonly ISettingsService settingsService;

        public DashboardController(ISettingsService settingsService)
        {
            this.settingsService = settingsService;
        }

        public IActionResult Index()
        {
            var viewModel = new IndexViewModel { SettingsCount = settingsService.GetCount(), };
            return View(viewModel);
        }
    }
}
