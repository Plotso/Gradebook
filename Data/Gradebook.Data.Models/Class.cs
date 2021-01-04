namespace Gradebook.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Common.Models;

    public class Class : BaseDeletableModel<int>
    {
        public Class()
        {
            Students = new HashSet<Student>();
        }

        [Required]
        public char Letter { get; set; }

        public int Year { get; set; } // 1st year, 2nd year, 3rd year student and etc.

        public int YearCreated { get; set; }  // випуск

        public int TeacherId { get; set; }

        /// <summary>
        /// ClassTeacher/Tutor
        /// </summary>
        public virtual Teacher Teacher { get; set; }

        public virtual ICollection<Student> Students { get; set; }
    }
}