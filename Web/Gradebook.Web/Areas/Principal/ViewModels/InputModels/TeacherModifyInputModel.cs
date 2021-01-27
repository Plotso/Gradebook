namespace Gradebook.Web.Areas.Principal.ViewModels.InputModels
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Web.ViewModels.InputModels;

    public class TeacherModifyInputModel
    {
        public int Id { get; set; }

        public List<SelectListItem> Schools { get; set; }

        public TeacherInputModel Teacher { get; set; }
    }
}