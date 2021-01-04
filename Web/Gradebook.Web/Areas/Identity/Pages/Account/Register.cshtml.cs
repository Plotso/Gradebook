namespace Gradebook.Web.Areas.Identity.Pages.Account
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;
    using Castle.Core.Internal;
    using Common;
    using Data.Common.Models;
    using Data.Common.Repositories;
    using Data.Models;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.Extensions.Logging;
    using Services.Data.Interfaces;

    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly IEmailSender _emailSender;
        private readonly IDeletableEntityRepository<ApplicationUser> _usersRepository;
        private readonly IUsersService _usersService;
        private readonly ILogger<RegisterModel> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IDeletableEntityRepository<ApplicationUser> usersRepository,
            IUsersService usersService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _usersRepository = usersRepository;
            _usersService = usersService;
        }

        [BindProperty] public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            var usersCount = _usersRepository.All().Count();
            if (ModelState.IsValid && IsUniqueIDValid(usersCount, out UserType userType))
            {
                var user = new ApplicationUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    UniqueGradebookId = Input.UniqueGradebookId
                };

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (usersCount == 0)
                {
                    // If no users are registered, make the first one an admin
                    await _userManager.AddToRoleAsync(user, GlobalConstants.AdministratorRoleName);
                }
                else
                {
                    switch (userType)
                    {
                        case UserType.Principal:
                            await _userManager.AddToRoleAsync(user, GlobalConstants.PrincipalRoleName);
                            break;
                        case UserType.Teacher:
                            await _userManager.AddToRoleAsync(user, GlobalConstants.TeacherRoleName);
                            break;
                        case UserType.Student:
                            await _userManager.AddToRoleAsync(user, GlobalConstants.StudentRoleName);
                            break;
                        case UserType.Parent:
                            await _userManager.AddToRoleAsync(user, GlobalConstants.ParentRoleName);
                            break;
                    }
                }

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        null,
                        new {area = "Identity", userId = user.Id, code, returnUrl},
                        Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new {email = Input.Email, returnUrl});
                    }

                    await _signInManager.SignInAsync(user, false);
                    return LocalRedirect(returnUrl);
                }

                foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [StringLength(50, MinimumLength = 5)]
            [Display(Name = "UniqueID")]
            public string UniqueGradebookId { get; set; } // Validated inside IsUniqueIDValid() method

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
                MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        /// <summary>
        /// Checks if UniqueId is for existing user type. The only exception is that initial user would not have existing UniqueID to assign himself to.
        /// </summary>
        /// <param name="usersCount">The number of already existing users.</param>
        /// <param name="userType">The type of user that should be registered.</param>
        /// <returns></returns>
        private bool IsUniqueIDValid(int usersCount, out UserType userType)
        {
            userType = UserType.None;
            if (usersCount > 0)
            {
                if (Input.UniqueGradebookId.IsNullOrEmpty())
                {
                    return false;
                }

                var dbUserType = _usersService.GetUserTypeByUniqueId(Input.UniqueGradebookId);
                if (dbUserType != UserType.None)
                {
                    userType = dbUserType;
                }
            }

            return true;
        }
    }
}