namespace Gradebook.Web.Areas.Teacher.ViewModels.InputModels
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Web.ViewModels.InputModels;

    public class AbsenceCreateInputModel
    {
        public AbsenceInputModel Absence { get; set; }
        
        public string StudentName { get; set; }
    }
}