namespace Gradebook.Web.ViewModels.Principal
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Data.Models;
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
    }
}