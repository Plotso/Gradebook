namespace Gradebook.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Common;
    using Common.Models;

    public class Student : BasePersonModel
    {
        private const int PINLength = 10;

        public Student()
        {
            UserType = UserType.Student;
            StudentParents = new HashSet<StudentParent>();
            StudentSubjects = new HashSet<StudentSubject>();
        }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Username { get; set; }  // ToDo: Username is not required here, can be populated on user registration

        public string Email { get; set; } // ToDo: Username is not required here

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        [StringLength(PINLength, MinimumLength = PINLength)]
        public string PersonalIdentificationNumber { get; set; } //ToDo: Add custom validation attribute

        public int? ClassId { get; set; } // nullable because university students don't have class

        public virtual Class Class { get; set; }

        public int SchoolId { get; set; }

        public virtual School School { get; set; }

        public virtual ICollection<StudentParent> StudentParents { get; set; }

        public virtual ICollection<StudentSubject> StudentSubjects { get; set; }
    }
}