using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using SaloonReservation.Data;

namespace SaloonReservation.Areas.Barber.Pages.Appointments
{
    public class IndexModel : PageModel
    {
        private SalonContext _context;

        public string url { get; set; }
        private readonly UserManager<ApplicationUser> _userManager;
        public int BarberId { get; set; }
        public IndexModel(SalonContext context, IWebHostEnvironment hostEnvironment, UserManager<ApplicationUser> userManager,
                                            IToastNotification toastNotification)
        {
            _context = context;
            _userManager = userManager;

        }
        public async Task<IActionResult> OnGet()
        {
            url = $"{this.Request.Scheme}://{this.Request.Host}";
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return Redirect("/Login");
            }
            var barber = _context.Barbers.Where(e => e.Email == user.Email).FirstOrDefault();
            if (barber is null)
            {
                return Redirect("/Login");

            }
            BarberId = barber.BarberId;
            return Page();
        }
    }
}


