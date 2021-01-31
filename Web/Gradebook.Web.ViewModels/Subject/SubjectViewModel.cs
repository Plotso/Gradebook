namespace Gradebook.Web.ViewModels.Subject
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Data.Models;
    using Services.Mapping;

    public class SubjectViewModel : IMapFrom<Subject>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int YearGrade { get; set; }

        public string SchoolYear { get; set; }
        
        public int TeacherId { get; set; }

        public string TeacherFirstName { get; set; }

        public string TeacherLastName { get; set; }
        
        public string TeacherSchoolName { get; set; }
        
        public string TeacherUniqueId { get; set; }

        public ICollection<StudentSubjectViewModel> StudentSubjects { get; set; }

        public string TeacherFullName => $"{TeacherFirstName} {TeacherLastName}";
    }
}