namespace Gradebook.Web.ViewModels.Home
{
    using System.Collections.Generic;
    using Data.Models;
    using Principal;
    using Services.Mapping;

    public class SchoolViewModel : IMapFrom<School>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public SchoolType Type { get; set; }

        public string SchoolImageName { get; set; }

        public string PrincipalFirstName { get; set; }

        public string PrincipalLastName { get; set; }

        public IEnumerable<ClassViewModel> Classes { get; set; }

        public string PrincipalFullName => PrincipalFirstName + " " + PrincipalLastName;
    }
}