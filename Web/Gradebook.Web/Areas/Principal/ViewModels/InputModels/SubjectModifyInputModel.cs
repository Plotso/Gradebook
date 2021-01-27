namespace Gradebook.Web.Areas.Principal.ViewModels.InputModels
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Web.ViewModels.Subject;

    public class SubjectModifyInputModel
    {
        public int Id { get; set; }
        
        public List<SelectListItem> Teachers { get; set; }
        
        public SubjectInputModel Subject { get; set; }
    }
}