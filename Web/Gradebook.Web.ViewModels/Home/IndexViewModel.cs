namespace Gradebook.Web.ViewModels.Home
{
    using System.Collections.Generic;

    public class IndexViewModel
    {
        public string Username { get; set; }
        
        public IEnumerable<SchoolViewModel> Schools { get; set; }
    }
}