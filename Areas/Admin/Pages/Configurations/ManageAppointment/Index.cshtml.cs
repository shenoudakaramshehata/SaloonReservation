using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaloonReservation.Data;
using SaloonReservation.Models;
using NToastNotify;
using Microsoft.AspNetCore.Localization;
using SaloonReservation.Migrations.Salon;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace SaloonReservation.Areas.Admin.Pages.Configurations.ManageAppointment
{
    public class IndexModel : PageModel
    {
        private SalonContext _context;
        
        public string url { get; set; }

        public IndexModel(SalonContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
         

        }
        public async Task<IActionResult> OnGet()
        {
           
            return Page();
        }
    }
}
