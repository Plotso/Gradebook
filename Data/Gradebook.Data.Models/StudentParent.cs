namespace Gradebook.Data.Models
{
    using Common.Models;

    public class StudentParent : BaseDeletableModel<int>
    {
        public int StudentId { get; set; }

        public virtual Student Student { get; set; }

        public int ParentId { get; set; }

        public virtual Parent Parent { get; set; }
    }
}