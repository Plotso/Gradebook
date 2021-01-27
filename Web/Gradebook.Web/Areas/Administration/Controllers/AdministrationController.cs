namespace Gradebook.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Castle.Core.Internal;
    using Common;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Principal;
    using Services.Interfaces;
    using ViewModels;
    using ViewModels.InputModels;
    using Web.Controllers;
    using Web.ViewModels.Administration;
    using Web.ViewModels.Home;

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

        public IActionResult SchoolsList()
        {
            var viewModel = new SchoolsListViewModel
            {
                Schools = _schoolsServices.GetAll<SchoolViewModel>()
            };

            return View(viewModel);
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

                return RedirectToAction(nameof(ConfirmCreated), confirmViewModel);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An exception occured during new school/principal record creation.");
                return RedirectToAction("Error", "Home");
            }
        }

        public IActionResult EditSchool(int id)
        {
            var schoolInputModel = _schoolsServices.GetById<SchoolInputModel>(id);
            var inputModel = new SchoolModifyInputModel
            {
                Id = id,
                School = schoolInputModel
            };

            return View(inputModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSchool(SchoolModifyInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return View(inputModel);
            }

            try
            {
                await _schoolsServices.EditAsync(inputModel);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An exception occured during school record UPDATE operation for school with ID: {inputModel.Id}");
                return RedirectToAction("Error", "Home");
            }

            return RedirectToAction(nameof(SchoolsList));
        }

        public IActionResult DeleteSchool(int id)
        {
            var schoolInputModel = _schoolsServices.GetById<SchoolInputModel>(id);
            var inputModel = new SchoolModifyInputModel
            {
                Id = id,
                School = schoolInputModel
            };

            return View(inputModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSchool(SchoolModifyInputModel inputModel, string onSubmitAction)
        {
            if (onSubmitAction.IsNullOrEmpty() || onSubmitAction == "Cancel")
            {
                return RedirectToAction(nameof(SchoolsList));
            }

            if (!ModelState.IsValid)
            {
                return View(inputModel);
            }

            try
            {
                await _schoolsServices.DeleteAsync(inputModel.Id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An exception occured during school record DELETE operation for school with ID: {inputModel.Id}");
                return RedirectToAction("Error", "Home");
            }

            return RedirectToAction(nameof(SchoolsList));
        }

        public IActionResult ConfirmCreated(ConfirmCreatedViewModel viewModel)
        {
            return View(viewModel);
        }
    }
}
