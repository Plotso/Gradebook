namespace Gradebook.Web.ViewModels.Subject
{
    using System.Collections.Generic;
    using System.Linq;

    public class SubjectsListViewModel
    {
        public IEnumerable<SubjectViewModel> Subjects { get; set; }
        
        public Dictionary<string, List<SubjectViewModel>> SubjectsBySchool
            => Subjects.GroupBy(s => s.TeacherSchoolName).ToList().ToDictionary(g => g.Key, g => g.ToList());
    }
}