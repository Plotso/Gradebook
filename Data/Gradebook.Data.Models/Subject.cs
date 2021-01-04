﻿namespace Gradebook.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Subject
    {
        public Subject()
        {
            StudentSubjects = new HashSet<StudentSubject>();
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int YearGrade { get; set; }  // for instance 3rd grade/year students

        [Required]
        public string SchoolYear { get; set; }  // ex: 2018-2019

        public int TeacherId { get; set; }

        public virtual Teacher Teacher { get; set; }

        public virtual ICollection<StudentSubject> StudentSubjects { get; set; }
    }
}