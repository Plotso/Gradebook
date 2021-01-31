namespace Gradebook.Web.Areas.Teacher.ViewModels.InputModels
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Web.ViewModels.InputModels;

    public class GradeCreateInputModel
    {
        public GradeInputModel Grade { get; set; }
        
        public string StudentName { get; set; }
    }
}