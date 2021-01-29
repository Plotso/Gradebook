namespace Gradebook.Web.ViewModels
{
    using Data.Models;
    using Services.Mapping;

    public class StudentParentViewModel : IMapFrom<StudentParent>
    {
        public StudentViewModel Student { get; set; }
        
        public ParentViewModel Parent { get; set; }
    }
}