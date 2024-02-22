using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaloonReservation.Data;
using SaloonReservation.Models;

namespace SaloonReservation.Areas.Barber.Pages
{
    public class IndexModel : PageModel
    {
        private SalonContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public int CustomerCount { get; set; }
        public int NewAppointmentCount { get; set; }
        public double NewAppointmentAmount { get; set; }
        public int CanceledAppointmentCount { get; set; }
        public double CanceledAppointmentAmount { get; set; }
        public int ClosedAppointmentCount { get; set; }
        public double ClosedAppointmentAmount { get; set; }
        public int TotalAppointmentCount { get; set; }
        public double TotalAppointmentAmount { get; set; }
        public IndexModel(SalonContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> OnGet()
        {
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
            CustomerCount = _context.Customers.Count();
            TotalAppointmentCount = _context.Appointments.Where(e=>e.BaberId==barber.BarberId).Count();
            TotalAppointmentAmount = _context.Appointments.Where(e => e.BaberId == barber.BarberId).Sum(e => e.TotalAmount.Value);
            NewAppointmentCount = _context.Appointments.Where(e => e.AppointmentStatusId == 1&& e.BaberId == barber.BarberId).Count();
            NewAppointmentAmount = _context.Appointments.Where(e => e.AppointmentStatusId == 1&& e.BaberId == barber.BarberId).Sum(e => e.TotalAmount.Value);
            CanceledAppointmentCount = _context.Appointments.Where(e => e.AppointmentStatusId == 2&& e.BaberId == barber.BarberId).Count();
            CanceledAppointmentAmount = _context.Appointments.Where(e => e.AppointmentStatusId == 2&& e.BaberId == barber.BarberId).Sum(e => e.TotalAmount.Value);
            ClosedAppointmentCount = _context.Appointments.Where(e => e.AppointmentStatusId == 3 && e.BaberId == barber.BarberId).Count();
            ClosedAppointmentAmount = _context.Appointments.Where(e => e.AppointmentStatusId == 3 && e.BaberId == barber.BarberId).Sum(e => e.TotalAmount.Value);
            return Page();
        }

    }
}

