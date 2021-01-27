namespace Gradebook.Web.Areas.Administration.ViewModels.InputModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Data.Models;
    using Gradebook.Services.Mapping;
    using Microsoft.AspNetCore.Http;

    // File is here due to direct dependency to Microsoft.AspNetCore.Http
    public class SchoolInputModel : IMapFrom<School>, IMapTo<School>, IValidatableObject
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public SchoolType Type { get; set; }

        public IFormFile SchoolImage { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validImageExtensions = new [] { ".jpeg", ".jpg", ".png", ".gif" };
            if (SchoolImage != null && !validImageExtensions.Any(x => SchoolImage.FileName.EndsWith(x)))
            {
                yield return new ValidationResult("Valid file extensions for an image are .jpeg/jpg/png/gif");
            }
        }
    }
}