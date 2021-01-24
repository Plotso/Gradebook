namespace Gradebook.Web.ViewModels.Subject
{
    using System.ComponentModel.DataAnnotations;
    using Data.Models;
    using Services.Mapping;

    public class SubjectInputModel : IMapFrom<Subject>, IMapTo<Subject>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [Range(1, 12)]
        public int YearGrade { get; set; }  // for instance 3rd grade/year students

        [Required]
        public string SchoolYear { get; set; }  // ex: 2018-2019
        
        [Required]
        public string TeacherId { get; set; }
    }
}