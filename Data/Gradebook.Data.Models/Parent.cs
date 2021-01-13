namespace Gradebook.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Common;
    using Common.Models;

    public class Parent : BasePersonModel
    {
        public Parent()
        {
            UserType = UserType.Parent;
        }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public int StudentId { get; set; }

        public virtual ICollection<StudentParent> StudentParents { get; set; }
    }
}