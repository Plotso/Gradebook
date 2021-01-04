namespace Gradebook.Web.Areas.Principal
{
    using Common;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.PrincipalRoleName)]
    [Area("Principal")]
    public class ManagementController
    {
        //ToDo: Add logic for creation of new Teacher/Student/Subject/Parent
    }
}