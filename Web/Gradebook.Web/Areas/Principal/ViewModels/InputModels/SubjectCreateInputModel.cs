namespace Gradebook.Web.Areas.Principal.ViewModels.InputModels
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Web.ViewModels.Subject;

    public class SubjectCreateInputModel
    {
        public List<SelectListItem> Teachers { get; set; }
        
        public SubjectInputModel Subject { get; set; }
    }
}