namespace Gradebook.Web.ViewModels.InputModels
{
    using System.ComponentModel.DataAnnotations;
    using Data.Models;
    using Services.Mapping;

    public class SchoolInputModel : IMapFrom<School>, IMapTo<School>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public SchoolType Type { get; set; }

        [Required]
        public string SchoolImageName { get; set; } = "school.jpg";
    }
}