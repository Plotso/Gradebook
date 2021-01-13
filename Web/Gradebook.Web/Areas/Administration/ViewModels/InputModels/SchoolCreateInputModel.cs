namespace Gradebook.Web.Areas.Administration.ViewModels.InputModels
{
    using Web.ViewModels.InputModels;

    public class SchoolCreateInputModel
    {
        public SchoolInputModel School { get; set; }

        public PrincipalInputModel Principal { get; set; }
    }
}