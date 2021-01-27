namespace Gradebook.Web.ViewModels.Subject
{
    using System.Collections.Generic;
    using Data.Models;
    using Services.Mapping;

    public class StudentSubjectViewModel : IMapFrom<StudentSubject>
    {
        public string StudentFirstName { get; set; }

        public string StudentLastName { get; set; }

        public ICollection<GradeViewModel> Grades { get; set; }

        public string StudentFullName => $"{StudentFirstName} {StudentLastName}";
    }
}