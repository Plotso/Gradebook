namespace Gradebook.Web.ViewModels.Principal
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using Data.Models;
    using Data.Models.Grades;
    using Microsoft.EntityFrameworkCore.Internal;
    using Services.Mapping;

    public class ClassViewModel : IMapFrom<Class>
    {
        public int Id { get; set; }

        public char Letter { get; set; }

        public int Year { get; set; } // 1st year, 2nd year, 3rd year student and etc.

        public int YearCreated { get; set; }  // випуск
        
        public TeacherViewModel Teacher { get; set; }
        
        public ICollection<StudentViewModel> Students { get; set; }
        
        public string TeacherFullName => $"{Teacher.FirstName} {Teacher.LastName}";

        public decimal? AverageGradeFirstTerm => GetAverageGradeByPeriod(GradePeriod.FirstTerm);
        
        public decimal? AverageGradeSecondTerm => GetAverageGradeByPeriod(GradePeriod.SecondTerm);

        private decimal? GetAverageGradeByPeriod(GradePeriod period)
        {
            if (period == GradePeriod.FirstTerm)
            {
                if (Students.Any(s => s.AverageGradeFirstTerm != null))
                {
                    return Students.Where(s => s.AverageGradeFirstTerm != null).Average(s => s.AverageGradeFirstTerm);
                }
            }
            else
            {
                if (Students.Any(s => s.AverageGradeSecondTerm != null))
                {
                    return Students.Where(s => s.AverageGradeSecondTerm != null).Average(s => s.AverageGradeSecondTerm);
                }
            }

            return null;
        }
    }
}