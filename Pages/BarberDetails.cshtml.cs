using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using SaloonReservation.Data;
using SaloonReservation.Models;

namespace SaloonReservation.Pages
{
    public class BarberDetailsModel : PageModel
    {
        private SalonContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IToastNotification _toastNotification;
        public Barber? Barber {  get; set; }
        public BarberDetailsModel(SalonContext context, IWebHostEnvironment hostEnvironment,
                                           IToastNotification toastNotification)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _toastNotification = toastNotification;

        }
        public IActionResult OnGet(int id)
        {
            Barber = _context.Barbers
                .Where(b => b.BarberId == id && b.IsActive)
                .Include(b => b.BarberImages)
                .Include(b => b.appoitments)
                .FirstOrDefault();
            if (Barber == null)
            {
                return Redirect("/Index");
            }
            return Page();
        }
    }
}
