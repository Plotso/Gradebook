namespace Gradebook.Web.ViewModels.Principal
{
    using Data.Models;
    using Services.Mapping;

    public class TeacherViewModel : IMapFrom<Teacher>
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
        
        public int SchoolId { get; set; }
        
        public string SchoolName { get; set; }
        
        public string UniqueId { get; set; }
    }
}