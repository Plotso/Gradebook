namespace Gradebook.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Common;
    using Data.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Services.Interfaces;
    using ViewModels.Home;
    using ViewModels.Subject;

    public class SubjectsController : Controller
    {
        private readonly ILogger<SubjectsController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISubjectsService _subjectsService;
        private readonly ITeachersService _teachersService;
        private readonly IStudentsService _studentsService;
        private readonly ISchoolsServices _schoolsServices;

        public SubjectsController(
            ILogger<SubjectsController> logger,
            UserManager<ApplicationUser> userManager,
            ISubjectsService subjectsService,
            ITeachersService teachersService,
            IStudentsService studentsService,
            ISchoolsServices schoolsServices)
        {
            _logger = logger;
            _userManager = userManager;
            _subjectsService = subjectsService;
            _teachersService = teachersService;
            _studentsService = studentsService;
            _schoolsServices = schoolsServices;
        }

        public async Task<IActionResult> SubjectsList()
        {
            var viewModel = new SubjectsListViewModel();
            var subjects = await GetSubjectsBasedOnUserRoleAsync();
            if (subjects == null)
            {
                return RedirectToAction("Error", "Home");
            }

            viewModel.Subjects = subjects;

            return View(viewModel);
        }

        public IActionResult ById(int id)
        {
            var viewModel = _subjectsService.GetById<SubjectViewModel>(id);
            return View(viewModel);
        }

        private async Task<IEnumerable<SubjectViewModel>> GetSubjectsBasedOnUserRoleAsync()
        {
            if (User.IsInRole(GlobalConstants.TeacherRoleName))
            {
                var user = await _userManager.GetUserAsync(User);
                var teacherId = _teachersService.GetIdByUniqueId(user.UniqueGradebookId);
                if (teacherId == null)
                {
                    return null; // RedirectToAction("Error", "Home"); // Maybe add reasonable message?
                }

                return _subjectsService.GetAllByTeacherId<SubjectViewModel>(teacherId.Value);
            }

            if (User.IsInRole(GlobalConstants.StudentRoleName))
            {
                var user = await _userManager.GetUserAsync(User);
                var studentId = _studentsService.GetIdByUniqueId(user.UniqueGradebookId);
                if (studentId == null)
                {
                    return null;  // RedirectToAction("Error", "Home"); // Maybe add reasonable message?
                }

                return _subjectsService.GetAllByStudentId<SubjectViewModel>(studentId.Value);
            }

            if (User.IsInRole(GlobalConstants.PrincipalRoleName))
            {
                var user = await _userManager.GetUserAsync(User);
                var schoolIds = _schoolsServices.GetAllByUserId<SchoolViewModel>(user.UniqueGradebookId).Select(s => s.Id);
                if (!schoolIds.Any())
                {
                    return null;  // RedirectToAction("Error", "Home"); // Maybe add reasonable message?
                }

                return _subjectsService.GetAllByStudentId<SubjectViewModel>(schoolIds.FirstOrDefault());
            }

            return _subjectsService.GetAll<SubjectViewModel>();
        }
    }
}