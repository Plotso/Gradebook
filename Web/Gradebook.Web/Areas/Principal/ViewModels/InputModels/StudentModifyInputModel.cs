namespace Gradebook.Web.Areas.Principal.ViewModels.InputModels
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Web.ViewModels.InputModels;

    public class StudentModifyInputModel
    {
        public int Id { get; set; }
        
        public List<SelectListItem> Schools { get; set; }

        public List<SelectListItem> Classes { get; set; }

        public StudentInputModel Student { get; set; }
    }
}