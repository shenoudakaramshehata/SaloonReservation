using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using SaloonReservation.Data;
using SaloonReservation.Models;
using SaloonReservation.ViewModels;

namespace SaloonReservation.Pages
{
    public class BarbersModel : PageModel
    {
        private SalonContext _context;
        public List<Barber> Barbers { get; set; }


        public BarbersModel(SalonContext context)
        {
            _context = context;

        }
        public IActionResult OnGet()
        {
            Barbers = _context.Barbers.Where(e => e.IsActive == true).ToList();
            return Page();

        }
    }
}
