namespace Gradebook.Web.ViewModels
{
    using System;
    using System.Collections.Generic;
    using Data.Models;
    using Services.Mapping;

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
    }
}