namespace Gradebook.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Common;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Principal;
    using Services.Interfaces;
    using ViewModels.InputModels;
    using Web.Controllers;
    using Web.ViewModels.Administration;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class AdministrationController : BaseController
    {
        private readonly ILogger<AdministrationController> _logger;
        private readonly IUsersService _usersService;
        private readonly ISchoolsServices _schoolsServices;

        public AdministrationController(ILogger<AdministrationController> logger, IUsersService usersService, ISchoolsServices schoolsServices)
        {
            _logger = logger;
            _usersService = usersService;
            _schoolsServices = schoolsServices;
        }

        public IActionResult CreateSchool()
        {
            var inputModel = new SchoolCreateInputModel();
            return View(inputModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSchool(SchoolCreateInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return View(inputModel);
            }

            try
            {
                var confirmViewModel = await _usersService.CreatePrincipal<ConfirmCreatedViewModel>(inputModel.Principal);
                await _schoolsServices.Create(inputModel.School, confirmViewModel.UniqueId);

                return RedirectToAction("ConfirmCreated", "Home", confirmViewModel);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An exception occured during new school/principal record creation.");
                return RedirectToAction("Error", "Home");
            }

        }
    }
}
