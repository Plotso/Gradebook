namespace Gradebook.Web.ViewModels.InputModels
{
    using System.ComponentModel.DataAnnotations;
    using Data.Models;
    using Services.Mapping;

    public class PrincipalInputModel : IMapFrom<Principal>, IMapTo<Principal>
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
    }
}