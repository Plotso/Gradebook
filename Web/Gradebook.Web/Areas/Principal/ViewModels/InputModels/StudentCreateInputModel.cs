namespace Gradebook.Web.Areas.Principal.ViewModels.InputModels
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Web.ViewModels.Home;
    using Web.ViewModels.InputModels;
    using Web.ViewModels.Principal;

    public class StudentCreateInputModel
    {
        public List<SelectListItem> Schools { get; set; }

        public List<SelectListItem> Classes { get; set; }

        public StudentInputModel Student { get; set; }
    }
}