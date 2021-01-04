namespace Gradebook.Data.Models
{
    using System.Collections.Generic;
    using Common.Models;
    using Grades;

    public class StudentSubject : BaseDeletableModel<int>
    {
        public int StudentId { get; set; }

        public virtual Student Student { get; set; }

        public int SubjectId { get; set; }

        public virtual Subject Subject { get; set; }

        public virtual ICollection<Grade> Grades { get; set; } = new HashSet<Grade>();
    }
}