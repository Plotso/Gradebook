namespace Gradebook.Web.Areas.Administration.ViewModels
{
    using System.Collections.Generic;
    using Web.ViewModels.Home;

    public class SchoolsListViewModel
    {
        public IEnumerable<SchoolViewModel> Schools { get; set; }
    }
}