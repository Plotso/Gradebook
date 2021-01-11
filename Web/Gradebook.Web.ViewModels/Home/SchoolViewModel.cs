namespace Gradebook.Web.ViewModels.Home
{
    using Data.Models;
    using Services.Mapping;

    public class SchoolViewModel : IMapFrom<School>
    {
        public string Name { get; set; }

        public string Address { get; set; }

        public SchoolType Type { get; set; }

        public string SchoolImageName { get; set; }
        
        public string PrincipalFirstName { get; set; }
        
        public string PrincipalLastName { get; set; }

        public string PrincipalFullName => PrincipalFirstName + " " + PrincipalLastName;
    }
}