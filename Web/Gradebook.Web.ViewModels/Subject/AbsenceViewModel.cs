namespace Gradebook.Web.ViewModels.Subject
{
    using Data.Models.Absences;
    using Services.Mapping;

    public class AbsenceViewModel : IMapFrom<Absence>
    {
        public AbsencePeriod Period { get; set; }

        public AbsenceType Type { get; set; }
    }
}