namespace Gradebook.Web.ViewModels.InputModels
{
    using System.ComponentModel.DataAnnotations;
    using Data.Models;
    using Services.Mapping;

    public class TeacherInputModel : IMapFrom<Teacher>, IMapTo<Teacher>
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string SchoolId { get; set; }
    }
}