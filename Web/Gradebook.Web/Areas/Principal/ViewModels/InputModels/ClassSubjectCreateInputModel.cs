namespace Gradebook.Web.Areas.Principal.ViewModels.InputModels
{
    using System.Collections.Generic;
    using Data.Models;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Web.ViewModels.Principal;

    public class ClassSubjectCreateInputModel
    {
        public List<SelectListItem> Classes { get; set; }

        public List<SelectListItem> Subjects { get; set; }

        public ClassSubjectInputModel ClassSubjectPair { get; set; }
    }
}