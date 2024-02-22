using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using SaloonReservation.Data;
using SaloonReservation.Models;
using SaloonReservation.ViewModels;

namespace SaloonReservation.Pages
{
    public class IndexModel : PageModel
    {
		private readonly ILogger<IndexModel> _logger;
		private readonly SignInManager<ApplicationUser> _signInManager;

		private readonly IToastNotification _toastNotification;
		public IRequestCultureFeature locale;
		public string BrowserCulture;
		private readonly SalonContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		[BindProperty]
		public ContactUs ContactUs { get; set; }
		[BindProperty]
		public LoginVM loginVM { get; set; }

		public IndexModel(SignInManager<ApplicationUser> signInManager, IToastNotification toastNotification, ILogger<IndexModel> logger, SalonContext context, UserManager<ApplicationUser> userManager)
		{
			_signInManager = signInManager;
			_logger = logger;
			_toastNotification = toastNotification;
			_context = context;
			_userManager = userManager;
			loginVM = new LoginVM();
            ContactUs = new ContactUs();
		}


		public void OnGet()
		{
			
		}


        public async Task <IActionResult> OnPostLogin(string? returnUrl = null)
		{
			returnUrl ??= Url.Content("~/");
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(loginVM.Email);
				if (user == null)
				{
					ModelState.AddModelError(string.Empty, "Invalid login attempt.Make Sure Email And Password is Valid");
					return Page();
				}
				var result = await _signInManager.PasswordSignInAsync(loginVM.Email, loginVM.Password, loginVM.RememberMe, lockoutOnFailure: false);
				if (result.Succeeded)
				{
					if (await _userManager.IsInRoleAsync(user, "admin"))
					{


						return Redirect("/Admin/Index");
					}
					else if (await _userManager.IsInRoleAsync(user, "Barber"))
					{
						return Redirect("/Barber/Index");
					}
					else
					{
						return Redirect("~" + returnUrl);
					}

				}

				if (result.RequiresTwoFactor)
				{
					return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = loginVM.RememberMe });
				}
				if (result.IsLockedOut)
				{
					_logger.LogWarning("User account locked out.");
					return RedirectToPage("./Lockout");
				}
				else
				{
					ModelState.AddModelError(string.Empty, "Invalid login attempt.Make Sure Email And Password is Valid");
					return Page();
				}
			}

			return Redirect("/Index");
        }
        public IActionResult OnPostAddContactUS(string name, string email, string message)
		{
			try
			{
				var contactus = new ContactUs()
				{
					SendingDate = DateTime.Now,
					Email = email,
					Message = message,
					Name = name
				};

				_context.contactUs.Add(contactus);
				_context.SaveChanges();
				_toastNotification.AddSuccessToastMessage("Message Send successfully");
			}
			catch (Exception e)
			{
				_toastNotification.AddErrorToastMessage(e.Message);
			}
			return Redirect("/Index");
		}
	}
}