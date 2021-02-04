namespace Gradebook.Web.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data.Models;
    using Data.Models.Grades;
    using Microsoft.EntityFrameworkCore.Internal;
    using Services.Mapping;
    using Subject;

    public class StudentViewModel : IMapFrom<Student>
    {
        public int Id { get; set; }
        
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public DateTime BirthDate { get; set; }

        public string PersonalIdentificationNumber { get; set; }
        
        public string SchoolName { get; set; }
        
        public ICollection<StudentParentViewModel> StudentParents { get; set; }
        
        public ICollection<StudentSubjectViewModel> StudentSubjects { get; set; }
        
        public string UniqueId { get; set; }

        public decimal? AverageGradeFirstTerm => GetAverageGradeByPeriod(GradePeriod.FirstTerm);
        
        public decimal? AverageGradeSecondTerm => GetAverageGradeByPeriod(GradePeriod.SecondTerm);

        private decimal? GetAverageGradeByPeriod(GradePeriod period)
        {
            if (StudentSubjects.Any())
            {
                var studentSubjectWithGradeInTerm = StudentSubjects.Where(s => s.Grades.Any(g => g.Period == period)).ToList();
                if (studentSubjectWithGradeInTerm.Any())
                {
                    return studentSubjectWithGradeInTerm.Average(s =>
                        s.Grades.Where(g => g.Period == period).Average(g => g.Value));
                }
            }

            return null;
        }
    }
}