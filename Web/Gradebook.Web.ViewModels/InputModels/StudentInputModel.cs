namespace Gradebook.Web.ViewModels.InputModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Data.Models;
    using Home;
    using Principal;
    using Services.Mapping;

    public class StudentInputModel : IMapFrom<Student>, IMapTo<Student>
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 10)]
        public string PersonalIdentificationNumber { get; set; }

        [Required]
        public string ClassId { get; set; }

        [Required]
        public string SchoolId { get; set; }
    }
}