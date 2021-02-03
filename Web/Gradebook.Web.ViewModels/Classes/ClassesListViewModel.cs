namespace Gradebook.Web.ViewModels.Classes
{
    using System.Collections.Generic;
    using System.Linq;
    using Data.Models;
    using Principal;

    public class ClassesListViewModel
    {
        public IEnumerable<ClassViewModel> Classes { get; set; }

        public Dictionary<string, List<ClassViewModel>> ClassesBySchool 
            => Classes.GroupBy(c => c.Teacher.SchoolName).ToList().ToDictionary(g => g.Key, g => g.ToList());
    }
}