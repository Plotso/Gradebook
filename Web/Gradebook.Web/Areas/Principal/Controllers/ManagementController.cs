namespace Gradebook.Web.Areas.Principal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml.Schema;
    using Castle.Core.Internal;
    using Castle.Core.Logging;
    using Common;
    using Data.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.Extensions.Logging;
    using Services.Interfaces;
    using ViewModels.InputModels;
    using Web.ViewModels;
    using Web.ViewModels.Administration;
    using Web.ViewModels.Home;
    using Web.ViewModels.InputModels;
    using Web.ViewModels.Principal;
    using Web.ViewModels.Subject;

    [Authorize(Roles = GlobalConstants.PrincipalRoleName + "," + GlobalConstants.AdministratorRoleName)]
    [Area("Principal")]
    public class ManagementController : Controller
    {
        private readonly ILogger<ManagementController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISchoolsServices _schoolsServices;
        private readonly IStudentsService _studentsService;
        private readonly ITeachersService _teachersService;
        private readonly ISubjectsService _subjectsService;
        private readonly IParentsService _parentsService;

        public ManagementController(
            ILogger<ManagementController> logger,
            UserManager<ApplicationUser> userManager,
            ISchoolsServices schoolsServices,
            IStudentsService studentsService,
            ITeachersService teachersService,
            ISubjectsService subjectsService,
            IParentsService parentsService)
        {
            _logger = logger;
            _userManager = userManager;
            _schoolsServices = schoolsServices;
            _studentsService = studentsService;
            _teachersService = teachersService;
            _subjectsService = subjectsService;
            _parentsService = parentsService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CreateSubject()
        {
            var user = await _userManager.GetUserAsync(User);
            var isAdmin = await IsAdmin();
            var schoolIds = _schoolsServices.GetAllByUserId<SchoolViewModel>(user?.UniqueGradebookId, isAdmin).Select(s => s.Id);
            var teachersInSchools = _teachersService.GetAllBySchoolIds<TeacherViewModel>(schoolIds);
            var inputModel = new SubjectCreateInputModel
            {
                Teachers = teachersInSchools.Select(t => new SelectListItem($"{t.FirstName} {t.LastName}", t.Id.ToString())).ToList()
            };
            return View(inputModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSubject(SubjectCreateInputModel inputModel)
        {
            if (!ModelState.IsValid || inputModel.Subject.TeacherId.IsNullOrEmpty())
            {
                //ToDo: in case of null teacher, return appropriate message or add model validation?
                return View(inputModel);
            }

            try
            {
               await _subjectsService.CreateSubject(inputModel.Subject);

               return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An exception occured during new subject record creation. Ex: {e.Message}");
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> EditSubject(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var isAdmin = await IsAdmin();
            var schoolIds = _schoolsServices.GetAllByUserId<SchoolViewModel>(user?.UniqueGradebookId, isAdmin).Select(s => s.Id);
            var teachersInSchools = _teachersService.GetAllBySchoolIds<TeacherViewModel>(schoolIds);
            var subject = _subjectsService.GetById<SubjectInputModel>(id);
            var inputModel = new SubjectModifyInputModel()
            {
                Id = id,
                Teachers = teachersInSchools.Select(t => new SelectListItem($"{t.FirstName} {t.LastName}", t.Id.ToString())).ToList(),
                Subject = subject
            };
            return View(inputModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSubject(SubjectModifyInputModel inputModel)
        {
            if (!ModelState.IsValid || inputModel.Subject.TeacherId.IsNullOrEmpty())
            {
                //ToDo: in case of null teacher, return appropriate message or add model validation?
                return View(inputModel);
            }

            try
            {
                await _subjectsService.EditAsync(inputModel);

                return RedirectToAction("ById", "Subjects", new { area = string.Empty, id = inputModel.Id });
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An exception occured during student UPDATE operation for subject with id {inputModel.Id}. Ex: {e.Message}");
                return RedirectToAction("Error", "Home");
            }
        }

        public IActionResult DeleteSubject(int id)
        {
            var subject = _subjectsService.GetById<SubjectInputModel>(id);
            var inputModel = new SubjectModifyInputModel()
            {
                Id = id,
                Subject = subject
            };
            return View(inputModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSubject(SubjectModifyInputModel inputModel, string onSubmitAction)
        {
            if (onSubmitAction.IsNullOrEmpty() || onSubmitAction == "Cancel")
            {
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                return View(inputModel);
            }

            try
            {
                await _subjectsService.DeleteAsync(inputModel.Id);

                return RedirectToAction("SubjectsList", "Subjects", new { area = string.Empty });  // string is empty in order to exit current principal area
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An exception occured during student DELETE operation for subject with id {inputModel.Id}. Ex: {e.Message}");
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> CreateParent()
        {
            var user = await _userManager.GetUserAsync(User);
            var isAdmin = await IsAdmin();
            var schools = _schoolsServices.GetAllByUserId<SchoolViewModel>(user?.UniqueGradebookId, isAdmin).ToList();
            var students = _studentsService.GetAllBySchoolIds<StudentViewModel>(schools.Select(s => s.Id));
            var inputModel = new ParentCreateInputModel
            {
                Students = students.Select(s => new SelectListItem($"{s.FirstName} {s.LastName} ({s.SchoolName})", s.Id.ToString())).ToList()
            };
            return View(inputModel);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateParent(ParentCreateInputModel inputModel)
        {
            if (!ModelState.IsValid || !inputModel.Parent.StudentIds.Any())
            {
                //ToDo: in case of null school/class, return appropriate message or add model validation?
                return View(inputModel);
            }

            try
            {
                var confirmViewModel = await _parentsService.CreateParentAsync<ConfirmCreatedViewModel>(inputModel.Parent);

                return RedirectToAction(nameof(ConfirmCreated), confirmViewModel);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An exception occured during new parent record creation. Ex: {e.Message}");
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> CreateStudent()
        {
            var user = await _userManager.GetUserAsync(User);
            var isAdmin = await IsAdmin();
            var schools = _schoolsServices.GetAllByUserId<SchoolViewModel>(user?.UniqueGradebookId, isAdmin);
            var inputModel = new StudentCreateInputModel
            {
                Schools = schools.Select(s => new SelectListItem(s.Name, s.Id.ToString())).ToList(),
                Classes = SetClasses(schools).ToList()
            };
            return View(inputModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateStudent(StudentCreateInputModel inputModel)
        {
            if (!ModelState.IsValid || inputModel.Student.SchoolId.IsNullOrEmpty() || inputModel.Student.ClassId.IsNullOrEmpty())
            {
                //ToDo: in case of null school/class, return appropriate message or add model validation?
                return View(inputModel);
            }

            try
            {
                var confirmViewModel = await _studentsService.CreateStudent<ConfirmCreatedViewModel>(inputModel.Student);

                return RedirectToAction(nameof(ConfirmCreated), confirmViewModel);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An exception occured during new student record creation. Ex: {e.Message}");
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> EditStudent(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var isAdmin = await IsAdmin();
            var schools = _schoolsServices.GetAllByUserId<SchoolViewModel>(user?.UniqueGradebookId, isAdmin);
            var student = _studentsService.GetById<StudentInputModel>(id);
            var inputModel = new StudentModifyInputModel()
            {
                Id = id,
                Schools = schools.Select(s => new SelectListItem(s.Name, s.Id.ToString())).ToList(),
                Classes = SetClasses(schools).ToList(),
                Student = student
            };
            return View(inputModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditStudent(StudentModifyInputModel inputModel)
        {
            if (!ModelState.IsValid || inputModel.Student.SchoolId.IsNullOrEmpty() || inputModel.Student.ClassId.IsNullOrEmpty())
            {
                //ToDo: in case of null school/class, return appropriate message or add model validation?
                return View(inputModel);
            }

            try
            {
                await _studentsService.EditAsync(inputModel);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An exception occured during student UPDATE operation for student with id {inputModel.Id}. Ex: {e.Message}");
                return RedirectToAction("Error", "Home");
            }
        }

        public IActionResult DeleteStudent(int id)
        {
            var student = _studentsService.GetById<StudentInputModel>(id);
            var inputModel = new StudentModifyInputModel()
            {
                Id = id,
                Student = student
            };
            return View(inputModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteStudent(StudentModifyInputModel inputModel, string onSubmitAction)
        {
            if (onSubmitAction.IsNullOrEmpty() || onSubmitAction == "Cancel")
            {
                return RedirectToAction(nameof(Index));
            }
            
            if (!ModelState.IsValid)
            {
                return View(inputModel);
            }

            try
            {
                await _studentsService.DeleteAsync(inputModel.Id);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An exception occured during student DELETE operation for student with id {inputModel.Id}. Ex: {e.Message}");
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> CreateTeacher()
        {
            var user = await _userManager.GetUserAsync(User);
            var isAdmin = await IsAdmin();
            var schools = _schoolsServices.GetAllByUserId<SchoolViewModel>(user?.UniqueGradebookId, isAdmin);
            var inputModel = new TeacherCreateInputModel
            {
                Schools = schools.Select(s => new SelectListItem(s.Name, s.Id.ToString())).ToList()
            };
            return View(inputModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTeacher(TeacherCreateInputModel inputModel)
        {
            if (!ModelState.IsValid || inputModel.Teacher.SchoolId.IsNullOrEmpty())
            {
                //ToDo: in case of null school/class, return appropriate message or add model validation?
                return View(inputModel);
            }

            try
            {
                var confirmViewModel = await _teachersService.CreateTeacher<ConfirmCreatedViewModel>(inputModel.Teacher);

                return RedirectToAction(nameof(ConfirmCreated), confirmViewModel);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An exception occured during new teacher record creation. Ex: {e.Message}");
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> EditTeacher(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var isAdmin = await IsAdmin();
            var schools = _schoolsServices.GetAllByUserId<SchoolViewModel>(user?.UniqueGradebookId, isAdmin);
            var teacher = _teachersService.GetById<TeacherInputModel>(id);
            var inputModel = new TeacherModifyInputModel
            {
                Id = id,
                Schools = schools.Select(s => new SelectListItem(s.Name, s.Id.ToString())).ToList(),
                Teacher = teacher
            };
            return View(inputModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTeacher(TeacherModifyInputModel inputModel)
        {
            if (!ModelState.IsValid || inputModel.Teacher.SchoolId.IsNullOrEmpty())
            {
                //ToDo: in case of null school/class, return appropriate message or add model validation?
                return View(inputModel);
            }

            try
            {
                await _teachersService.EditAsync(inputModel);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An exception occured during teacher UPDATE operation for teacher with id {inputModel.Id}. Ex: {e.Message}");
                return RedirectToAction("Error", "Home");
            }
        }

        public IActionResult DeleteTeacher(int id)
        {
            var teacher = _teachersService.GetById<TeacherInputModel>(id);
            var inputModel = new TeacherModifyInputModel
            {
                Id = id,
                Teacher = teacher
            };
            return View(inputModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTeacher(TeacherModifyInputModel inputModel, string onSubmitAction)
        {
            if (onSubmitAction.IsNullOrEmpty() || onSubmitAction == "Cancel")
            {
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                return View(inputModel);
            }

            try
            {
                await _teachersService.DeleteAsync(inputModel.Id);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An exception occured during teacher DELETE operation for teacher with id {inputModel.Id}. Ex: {e.Message}");
                return RedirectToAction("Error", "Home");
            }
        }

        public IActionResult ConfirmCreated(ConfirmCreatedViewModel viewModel)
        {
            return View(viewModel);
        }

        private IEnumerable<SelectListItem> SetClasses(IEnumerable<SchoolViewModel> schools)
        {
            var result = new List<SelectListItem>();
            foreach (var school in schools)
            {
                var schoolName = school.Name;
                foreach (var schoolClass in school.Classes)
                {
                    result.Add(new SelectListItem
                    {
                        Value = schoolClass.Id.ToString(),
                        Text = $"{schoolClass.Year}{schoolClass.Letter} - school {schoolName}"
                    });
                }
            }

            return result;
        }

        private async Task<bool> IsAdmin()
        {
            var user = await _userManager.GetUserAsync(User);
            var userRoles = await _userManager.GetRolesAsync(user);
            return userRoles.Any(r => r == GlobalConstants.AdministratorRoleName);
        }
    }
}
