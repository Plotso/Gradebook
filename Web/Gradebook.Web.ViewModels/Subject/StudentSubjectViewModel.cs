namespace Gradebook.Web.ViewModels.Subject
{
    using System.Collections.Generic;
    using System.Linq;
    using Data.Models;
    using Data.Models.Absences;
    using Data.Models.Grades;
    using Services.Mapping;

    public class StudentSubjectViewModel : IMapFrom<StudentSubject>
    {
        public int StudentId { get; set; }
        
        public string StudentFirstName { get; set; }

        public string StudentLastName { get; set; }

        public ICollection<GradeViewModel> Grades { get; set; }

        public ICollection<AbsenceViewModel> Absences { get; set; }

        public string StudentFullName => $"{StudentFirstName} {StudentLastName}";

        public Dictionary<AbsencePeriod, string> AbsencesByPeriod => Absences
            .GroupBy(a => a.Period)
            .ToList()
            .ToDictionary(g => g.Key, g => GetAbsencesForPeriod(g.Key));

        public Dictionary<GradePeriod, List<GradeViewModel>> GradesByPeriod => Grades
            .GroupBy(g => g.Period)
            .ToList()
            .ToDictionary(g => g.Key, g => Grades.Where(gr => gr.Period == g.Key).ToList());

        private string GetAbsencesForPeriod(AbsencePeriod period)
        {
            var absencesForPeriod = Absences.Where(a => a.Period == period).ToList();
            var fullAbsencesCount = absencesForPeriod.Count(a => a.Type == AbsenceType.Full);
            var thirdsCount = absencesForPeriod.Count(a => a.Type == AbsenceType.OneThird);

            var remainingThirds = thirdsCount % 3;
            var fullFromThirds = thirdsCount / 3;
            var actualFullCount = fullAbsencesCount + fullFromThirds;
            var result = "";
            if (actualFullCount != 0)
            {
                result += $"{actualFullCount}";
            }

            if (remainingThirds != 0)
            {
                result += $" {remainingThirds}/3";
            }

            return result;
        }
    }
}