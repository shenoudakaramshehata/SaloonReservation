using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaloonReservation.Data;
using SaloonReservation.Models;

namespace SaloonReservation.Areas.Admin.Pages
{
    public class IndexModel : PageModel
    {

        private SalonContext _context;
        public int CustomerCount { get; set; }
        public int NewAppointmentCount { get; set; }
        public double NewAppointmentAmount { get; set; }
        public int CanceledAppointmentCount { get; set; }
        public double CanceledAppointmentAmount { get; set; }
        public int ClosedAppointmentCount { get; set; }
        public double ClosedAppointmentAmount { get; set; }
        public int TotalAppointmentCount { get; set; }
        public double TotalAppointmentAmount { get; set; }
        public IndexModel(SalonContext context)
        {
            _context = context;
        }
        public void OnGet()
        {
            CustomerCount = _context.Customers.Count();
            TotalAppointmentCount = _context.Appointments.Count();
            TotalAppointmentAmount = _context.Appointments.Sum(e => e.TotalAmount.Value);
            NewAppointmentCount = _context.Appointments.Where(e => e.AppointmentStatusId == 1).Count();
            NewAppointmentAmount = _context.Appointments.Where(e => e.AppointmentStatusId == 1).Sum(e => e.TotalAmount.Value);
            CanceledAppointmentCount = _context.Appointments.Where(e => e.AppointmentStatusId == 2).Count();
            CanceledAppointmentAmount = _context.Appointments.Where(e => e.AppointmentStatusId == 2).Sum(e => e.TotalAmount.Value);
            ClosedAppointmentCount = _context.Appointments.Where(e => e.AppointmentStatusId == 3).Count();
            ClosedAppointmentAmount = _context.Appointments.Where(e => e.AppointmentStatusId == 3).Sum(e => e.TotalAmount.Value);
        }

    }
}

