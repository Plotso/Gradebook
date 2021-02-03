namespace Gradebook.Web.Areas.Principal.ViewModels.InputModels
{
    using System.Collections.Generic;

    using Gradebook.Web.ViewModels.InputModels;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class ParentModifyInputModel
    {
        public int Id { get; set; }

        public List<SelectListItem> Students { get; set; }
        
        public ParentInputModel Parent { get; set; }
    }
}