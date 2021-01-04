namespace Gradebook.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Common.Models;

    public class School : BaseDeletableModel<int>
    {
        public School()
        {
            Classes = new HashSet<Class>();
            Teachers = new HashSet<Teacher>();
            Students = new HashSet<Student>();
        }

        [Required]
        public string Name { get; set; }

        public string Address { get; set; }

        public SchoolType Type { get; set; }

        public int PrincipalId { get; set; }

        public virtual Principal Principal { get; set; }

        public virtual ICollection<Class> Classes { get; set; } //ToDo: Rename to Classes once seeding is completed

        public virtual ICollection<Teacher> Teachers { get; set; }

        public virtual ICollection<Student> Students { get; set; }
    }
}