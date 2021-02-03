namespace Gradebook.Web.Areas.Teacher.ViewModels.InputModels
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Web.ViewModels.InputModels;

    public class GradeModifyInputModel
    {
        public int Id { get; set; }

        public List<SelectListItem> Teachers { get; set; }

        public List<SelectListItem> StudentSubjectPairs { get; set; }

        public GradeInputModel Grade { get; set; }
    }
}