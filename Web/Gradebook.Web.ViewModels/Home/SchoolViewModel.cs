namespace Gradebook.Web.ViewModels.Home
{
    using System.Collections.Generic;
    using System.Linq;
    using Data.Models;
    using Data.Models.Grades;
    using Microsoft.EntityFrameworkCore.Internal;
    using Principal;
    using Services.Mapping;

    public class SchoolViewModel : IMapFrom<School>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public SchoolType Type { get; set; }

        public string SchoolImageName { get; set; }

        public string PrincipalFirstName { get; set; }

        public string PrincipalLastName { get; set; }

        public IEnumerable<ClassViewModel> Classes { get; set; }

        public string PrincipalFullName => PrincipalFirstName + " " + PrincipalLastName;

        public decimal? AverageGradeFirstTerm => GetAverageGradeByPeriod(GradePeriod.FirstTerm);

        public decimal? AverageGradeSecondTerm => GetAverageGradeByPeriod(GradePeriod.SecondTerm);

        private decimal? GetAverageGradeByPeriod(GradePeriod period)
        {
            if (period == GradePeriod.FirstTerm)
            {
                if (Classes.Any(s => s.AverageGradeFirstTerm != null))
                {
                    return Classes.Where(c => c.AverageGradeFirstTerm != null).Average(c => c.AverageGradeFirstTerm);
                }
            }
            else
            {
                if (Classes.Any(s => s.AverageGradeSecondTerm != null))
                {
                    return Classes.Where(c => c.AverageGradeSecondTerm != null).Average(c => c.AverageGradeSecondTerm);
                }
            }

            return null;
        }
    }
}