namespace Gradebook.Web.Areas.Principal.ViewModels.InputModels
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Web.ViewModels.Classes;

    public class ClassModifyInputModel
    {
        public int Id { get; set; }
        
        public List<SelectListItem> Teachers { get; set; }
        
        public ClassInputModel Class { get; set; }
    }
}