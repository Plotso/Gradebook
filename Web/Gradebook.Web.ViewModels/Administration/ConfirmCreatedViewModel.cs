namespace Gradebook.Web.ViewModels.Administration
{
    using Data.Common.Models;
    using Services.Mapping;

    public class ConfirmCreatedViewModel : IMapFrom<BasePersonModel>
    {
        public UserType UserType { get; set; }

        public string UniqueId { get; set; }
    }
}