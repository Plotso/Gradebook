namespace Gradebook.Data.Common.Models
{
    using System.ComponentModel.DataAnnotations;

    public class BasePersonModel : BaseDeletableModel<int>
    {
        [Required]
        public string UniqueId { get; set; }

        public UserType UserType { get; protected set; }
    }
}