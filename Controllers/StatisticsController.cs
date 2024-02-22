using SaloonReservation.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Microsoft.AspNetCore.Identity;

namespace SaloonReservation.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StatisticsController : Controller
    {
        private SalonContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;

        public StatisticsController(SalonContext context, UserManager<ApplicationUser> userManager, ApplicationDbContext db)
        {
            _context = context;
            _userManager = userManager;
            _db = db;

        }

        
        [HttpGet]
        public object GetAppointmentsPerStatus()
        {
            var data = _context.Appointments.Include(e=>e.AppointmentStatus)
                .GroupBy(c => c.AppointmentStatusId).

                Select(g => new
                {

                    //Lable = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key),
                    Lable = g.FirstOrDefault().AppointmentStatus.AppointmentStatusTitleAR,

                    Count = g.Count(),
                    Sum = g.Sum(e => e.TotalAmount)

                }).OrderByDescending(r => r.Sum).ToList();

            return data;


        }
        [HttpGet]
        public async Task<object> GetBarberAppointmentsPerStatus()
        {
            var user = await _userManager.GetUserAsync(User);
            var barber = _context.Barbers.Where(e => e.Email == user.Email).FirstOrDefault();
            var data = _context.Appointments.Where(e=>e.BaberId==barber.BarberId).Include(e => e.AppointmentStatus)
                .GroupBy(c => c.AppointmentStatusId).

                Select(g => new
                {

                    //Lable = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key),
                    Lable = g.FirstOrDefault().AppointmentStatus.AppointmentStatusTitleAR,

                    Count = g.Count(),
                    Sum = g.Sum(e => e.TotalAmount)

                }).OrderByDescending(r => r.Sum).ToList();

            return data;


        }

        [HttpGet]
        public async Task<object> GetAppointmentsPerMounth()
        {
           
            var data = _context.Appointments
                .GroupBy(c => c.AppointmentCreateDate.Value.Date.Month).

                Select(g => new
                {

                    Lable = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key),
                    Count = g.Count(),
                    Sum = g.Sum(e => e.TotalAmount)

                }).OrderByDescending(r => r.Sum).ToList();

            return data;


        }
        [HttpGet]
        public List<object> GetDountChart()
        {
            List<object> dataDount = new List<object>();
            List<string> labels = new List<string>();
            List<double> Revenue = new List<double>();
            var StatusList = _context.AppointmentStatuses.ToList();
            foreach (var item in StatusList)
            {
                labels.Add(item.AppointmentStatusTitleAR);
                double TemplatesRevenuePerCatagory = _context.Appointments.Where(e => e.AppointmentStatusId==item.AppointmentStatusId).Sum(e => e.TotalAmount.Value);
                Revenue.Add(TemplatesRevenuePerCatagory);
            }
            dataDount.Add(labels);
            dataDount.Add(Revenue);
            return dataDount;
        }
        [HttpGet]
        public async Task<List<object>> GetBarberDountChart()
        {
            var user = await _userManager.GetUserAsync(User);
            var barber = _context.Barbers.Where(e => e.Email == user.Email).FirstOrDefault();

            List<object> dataDount = new List<object>();
            List<string> labels = new List<string>();
            List<double> Revenue = new List<double>();
            var StatusList = _context.AppointmentStatuses.ToList();
            foreach (var item in StatusList)
            {
                labels.Add(item.AppointmentStatusTitleAR);
                double TemplatesRevenuePerCatagory = _context.Appointments.Where(e => e.AppointmentStatusId == item.AppointmentStatusId&&e.BaberId==barber.BarberId).Sum(e => e.TotalAmount.Value);
                Revenue.Add(TemplatesRevenuePerCatagory);
            }
            dataDount.Add(labels);
            dataDount.Add(Revenue);
            return dataDount;
        }
        [HttpGet]
        public async Task<object> GetBarberAppointmentsPerMounth()
        {
            var user = await _userManager.GetUserAsync(User);
            var barber = _context.Barbers.Where(e => e.Email == user.Email).FirstOrDefault();
            var data = _context.Appointments.Where(e=>e.BaberId== barber.BarberId)
                .GroupBy(c => c.AppointmentCreateDate.Value.Date.Month).

                Select(g => new
                {

                    Lable = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key),
                    Count = g.Count(),
                    Sum = g.Sum(e => e.TotalAmount)

                }).OrderByDescending(r => r.Sum).ToList();

            return data;


        }
    }
}
