namespace Gradebook.Web.Controllers
{
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Common;
    using Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Services.Data.Interfaces;
    using ViewModels;
    using ViewModels.Home;

    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ISchoolsServices _schoolsServices;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ISchoolsServices schoolsServices)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _schoolsServices = schoolsServices;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new IndexViewModel();

            if (_signInManager.IsSignedIn(User))
            {
                var user = await _userManager.GetUserAsync(User);
                viewModel.Username = user?.UserName;
                var isAdmin = await IsCurrentUserAdmin();
                if (isAdmin)
                {
                    viewModel.Schools = _schoolsServices.GetAll<SchoolViewModel>();
                }
                else
                {
                    viewModel.Schools = _schoolsServices.GetAllByUserId<SchoolViewModel>(user?.UniqueGradebookId);
                }
            }

            return View(viewModel);
        }

        public async Task<IActionResult> IndexWithViewModel()
        {
            var user = await _userManager.GetUserAsync(User);
            var viewModel = new IndexViewModel
            {
                Username = user.UserName
            };
            var isAdmin = await IsCurrentUserAdmin();
            if (isAdmin)
            {
                viewModel.Schools = _schoolsServices.GetAll<SchoolViewModel>();
            }
            else
            {
                viewModel.Schools = _schoolsServices.GetAllByUserId<SchoolViewModel>(user.UniqueGradebookId);
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult PageNotFound()
        {
            return View();
        }

        private async Task<bool> IsCurrentUserAdmin()
        {
            var user = await _userManager.GetUserAsync(User);
            var userRoles = await _userManager.GetRolesAsync(user);
            return userRoles.Any(r => r == GlobalConstants.AdministratorRoleName);
        }
    }
}
