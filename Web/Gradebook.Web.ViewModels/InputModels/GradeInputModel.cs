namespace Gradebook.Web.ViewModels.InputModels
{
    using System.ComponentModel.DataAnnotations;
    using Data.Models.Grades;
    using Services.Mapping;

    public class GradeInputModel : IMapFrom<Grade>, IMapTo<Grade>
    {
        [Required]
        [Range(2.00, 6.00)]
        public decimal Value { get; set; } //ex: 6.00, 4.50, 5.75

        [Required]
        public GradePeriod Period { get; set; }

        [Required]
        public GradeType Type { get; set; }

        public int StudentId { get; set; }

        public int SubjectId { get; set; }
        
        public int TeacherId { get; set; }
    }
}