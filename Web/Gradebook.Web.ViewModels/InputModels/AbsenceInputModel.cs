namespace Gradebook.Web.ViewModels.InputModels
{
    using System.ComponentModel.DataAnnotations;
    using Data.Models.Absences;
    using Services.Mapping;

    public class AbsenceInputModel : IMapFrom<Absence>, IMapTo<Absence>
    {
        [Required]
        public AbsencePeriod Period { get; set; }

        [Required]
        public AbsenceType Type { get; set; }

        public int StudentId { get; set; }

        public int SubjectId { get; set; }
        
        public int TeacherId { get; set; }
    }
}