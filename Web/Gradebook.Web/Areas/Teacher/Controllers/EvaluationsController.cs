namespace Gradebook.Web.Areas.Teacher.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Castle.Core.Internal;
    using Castle.Core.Logging;
    using Common;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Services.Interfaces;
    using ViewModels.InputModels;
    using Web.ViewModels;
    using Web.ViewModels.InputModels;

    [Authorize(Roles = GlobalConstants.PrincipalRoleName + "," + GlobalConstants.AdministratorRoleName + "," + GlobalConstants.TeacherRoleName)]
    [Area("Teacher")]
    public class EvaluationsController : Controller
    {
        private readonly ILogger<EvaluationsController> _logger;
        private readonly IAbsencesService _absencesService;
        private readonly IGradesService _gradesService;
        private readonly IStudentsService _studentsService;

        public EvaluationsController(ILogger<EvaluationsController> logger, IAbsencesService absencesService, IGradesService gradesService, IStudentsService studentsService)
        {
            _logger = logger;
            _absencesService = absencesService;
            _gradesService = gradesService;
            _studentsService = studentsService;
        }
    
        //ToDo: Add logic for Create, Edit, Delete Absences AND MARKS
        // ToDo: Add page where teacher can view marks and absences for a single student for a single subject (add validation for which teacher can see/edit which page)
        
        public async Task<IActionResult> AddGrade(int studentId, int subjectId, int teacherId)
        {
            var student = _studentsService.GetById<StudentViewModel>(studentId);
            var inputModel = new GradeCreateInputModel
            {
                Grade = new GradeInputModel
                {
                    TeacherId = teacherId,
                    StudentId = studentId,
                    SubjectId = subjectId,
                },
                StudentName = $"{student.FirstName} {student.LastName}"
            };

            return View(inputModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddGrade(GradeCreateInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return View(inputModel);
            }

            try
            {
                await _gradesService.CreateAsync(inputModel.Grade);

                return RedirectToAction("ById", "Subjects", new { area = string.Empty, id = inputModel.Grade.SubjectId });
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An exception occured during new grade record creation. Ex: {e.Message}");
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> AddAbsence(int studentId, int subjectId, int teacherId)
        {
            var student = _studentsService.GetById<StudentViewModel>(studentId);
            var inputModel = new AbsenceCreateInputModel()
            {
                Absence = new AbsenceInputModel
                {
                    TeacherId = teacherId,
                    StudentId = studentId,
                    SubjectId = subjectId,
                },
                StudentName = $"{student.FirstName} {student.LastName}"
            };

            return View(inputModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAbsence(AbsenceCreateInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return View(inputModel);
            }

            try
            {
                await _absencesService.CreateAsync(inputModel.Absence);

                return RedirectToAction("ById", "Subjects", new { area = string.Empty, id = inputModel.Absence.SubjectId });
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An exception occured during new absence record creation. Ex: {e.Message}");
                return RedirectToAction("Error", "Home");
            }
        }
    }
}