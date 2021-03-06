﻿namespace Gradebook.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using Common;
    using Common.Models;

    public class Teacher : BasePersonModel
    {
        public Teacher()
        {
            UserType = UserType.Teacher;
        }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Email { get; set; }

        public int SchoolId { get; set; }

        public virtual School School { get; set; }

        /// <summary>
        /// Populated only if teacher is ClassTeacher/Tutor of a specific class
        /// </summary>
        public virtual Class Class { get; set; }
    }
}