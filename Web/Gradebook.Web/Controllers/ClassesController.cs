namespace Gradebook.Web.Areas.Principal
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Common;
    using Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Services.Interfaces;
    using Web.ViewModels.Classes;
    using Web.ViewModels.Home;
    using Web.ViewModels.Principal;
    using Web.ViewModels.Subject;

    public class ClassesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IClassesService _classesService;
        private readonly ISchoolsServices _schoolsServices;
        private readonly ITeachersService _teachersService;
        private readonly IStudentsService _studentsService;
        private readonly IParentsService _parentsService;

        public ClassesController(
            UserManager<ApplicationUser> userManager,
            IClassesService classesService,
            ISchoolsServices schoolsServices,
            ITeachersService teachersService,
            IStudentsService studentsService,
            IParentsService parentsService)
        {
            _userManager = userManager;
            _classesService = classesService;
            _schoolsServices = schoolsServices;
            _teachersService = teachersService;
            _studentsService = studentsService;
            _parentsService = parentsService;
        }
        
        public async Task<IActionResult> ClassesList()
        {
            var viewModel = new ClassesListViewModel();
            var classes = await GetClassesBasedOnUserRoleAsync();
            if (classes == null)
            {
                return RedirectToAction("Error", "Home");
            }

            viewModel.Classes = classes;

            return View(viewModel);
        }

        public IActionResult ById(int id)
        {
            var viewModel = _classesService.GetById<ClassViewModel>(id);
            return View(viewModel);
        }
        
        private async Task<IEnumerable<ClassViewModel>> GetClassesBasedOnUserRoleAsync()
        {
            if (User.IsInRole(GlobalConstants.TeacherRoleName))
            {
                var user = await _userManager.GetUserAsync(User);
                var teacherId = _teachersService.GetIdByUniqueId(user.UniqueGradebookId);
                if (teacherId == null)
                {
                    return null; // RedirectToAction("Error", "Home"); // Maybe add reasonable message?
                }

                return _classesService.GetAllByTeacherId<ClassViewModel>(teacherId.Value);
            }

            if (User.IsInRole(GlobalConstants.StudentRoleName))
            {
                var user = await _userManager.GetUserAsync(User);
                var studentId = _studentsService.GetIdByUniqueId(user.UniqueGradebookId);
                if (studentId == null)
                {
                    return null;  // RedirectToAction("Error", "Home"); // Maybe add reasonable message?
                }

                return _classesService.GetAllByStudentId<ClassViewModel>(studentId.Value);
            }

            if (User.IsInRole(GlobalConstants.ParentRoleName))
            {
                var user = await _userManager.GetUserAsync(User);
                var studentIds = _parentsService.GetStudentIdsByParentUniqueId(user.UniqueGradebookId);
                if (!studentIds.Any())
                {
                    return null;  // RedirectToAction("Error", "Home"); // Maybe add reasonable message?
                }

                return _classesService.GetAllByMultipleStudentIds<ClassViewModel>(studentIds);
            }

            if (User.IsInRole(GlobalConstants.PrincipalRoleName))
            {
                var user = await _userManager.GetUserAsync(User);
                var schoolIds = _schoolsServices.GetAllByUserId<SchoolViewModel>(user.UniqueGradebookId).Select(s => s.Id);
                if (!schoolIds.Any())
                {
                    return null;  // RedirectToAction("Error", "Home"); // Maybe add reasonable message?
                }

                return _classesService.GetAllBySchoolId<ClassViewModel>(schoolIds.FirstOrDefault());
            }

            return _classesService.GetAll<ClassViewModel>();
        }
    }
}