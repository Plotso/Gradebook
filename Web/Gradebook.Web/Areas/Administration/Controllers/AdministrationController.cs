namespace Gradebook.Web.Areas.Administration.Controllers
{
    using Common;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Web.Controllers;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class AdministrationController : BaseController
    {
        //ToDo: Add logic for creation of new school & principal
        public IActionResult AddSchool()
        {
            return View();
        }
    }
}
