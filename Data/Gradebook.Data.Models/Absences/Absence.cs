namespace Gradebook.Data.Models.Absences
{
    using System.ComponentModel.DataAnnotations;
    using Common.Models;

    public class Absence : BaseDeletableModel<int>
    {
        [Required]
        public AbsencePeriod Period { get; set; }

        [Required]
        public AbsenceType Type { get; set; }

        [Required]
        public int TeacherId { get; set; }

        public virtual Teacher Teacher { get; set; }

        [Required]
        public int StudentSubjectId { get; set; }

        /// <summary>
        /// The StudentSubject is unique combination showing which student is taking which subject. Absences should be mapped to this pair.
        /// </summary>
        public virtual StudentSubject StudentSubject { get; set; }
    }
}