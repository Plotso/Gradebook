namespace Gradebook.Data.Models.Grades
{
    using System.ComponentModel.DataAnnotations;
    using Common.Models;

    public class Grade : BaseDeletableModel<int>
    {
        [Required]
        public decimal Value { get; set; } //ex: 6.00, 4.50, 5.75

        [Required]
        public GradePeriod Period { get; set; }

        [Required]
        public GradeType Type { get; set; }

        [Required]
        public int TeacherId { get; set; }

        public virtual Teacher Teacher { get; set; }

        [Required]
        public int StudentSubjectId { get; set; }

        /// <summary>
        /// The StudentSubject is unique combination showing which student is taking which subject. Grades should be mapped to this pair.
        /// </summary>
        public virtual StudentSubject StudentSubject { get; set; }
    }
}