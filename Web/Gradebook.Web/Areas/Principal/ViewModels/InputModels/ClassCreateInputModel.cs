namespace Gradebook.Web.Areas.Principal.ViewModels.InputModels
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Web.ViewModels.Classes;

    public class ClassCreateInputModel
    {
        public List<SelectListItem> Teachers { get; set; }
        
        public ClassInputModel Class { get; set; }
    }
}