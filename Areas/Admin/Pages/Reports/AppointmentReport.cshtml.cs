using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SaloonReservation.Data;
using SaloonReservation.ViewModels;
using SaloonReservation.Reports;
using Microsoft.EntityFrameworkCore;
using SaloonReservation.Models;
using Microsoft.AspNetCore.Localization;

namespace SaloonReservation.Areas.Admin.Pages.Reports
{
    public class AppointmentReportModel : PageModel
    {
        public SalonContext _context { get; set; }
        [BindProperty]
        public FilterModel filterModel { get; set; }

        public AppointmentReportModel(SalonContext context)
        {
            _context = context;
        }

        public AppointmentReportNewt Report { get; set; }

        public IRequestCultureFeature locale;
        public string BrowserCulture;
        public void OnGet()
        {
            locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            BrowserCulture = locale.RequestCulture.UICulture.ToString();
            List<SaloonReservation.ViewModels.AppointmentVmModel> ds = _context.Appointments.Include(a => a.Barber).Include(a => a.Customer).Include(a => a.AppointmentStatus).Include(a => a.Services).ThenInclude(e => e.Service).Select(i => new SaloonReservation.ViewModels.AppointmentVmModel
            {


                BarberId = i.BaberId.Value,
                BarberName = i.Barber.FullName,
                BarberPhone = i.Barber.Phone,
                CustomerName = i.Customer.FullName,
                TotalAmount = i.TotalAmount.Value,
                TotalDuration = i.Services.Sum(e => e.Service.Duration),
                Day = i.AppointmentCreateDate.Value.DayOfWeek.ToString(),
                AppointmentStartDate = i.AppointmentStartDate.Value.ToShortTimeString(),
                AppointmentEndDate = i.AppointmentEndDate.Value.ToShortTimeString(),
                AppointmentCreateDate = i.AppointmentCreateDate.Value.ToShortDateString(),
                AppointmentCreateDateNotFormat = i.AppointmentCreateDate.Value,
                AppointmentStatus = i.AppointmentStatus.AppointmentStatusTitleEN,

            }).ToList();
            Report = new AppointmentReportNewt();
            Report.DataSource = ds;
        }
        public IActionResult OnPost()
        {

            List<SaloonReservation.ViewModels.AppointmentVmModel> ds = _context.Appointments.Include(a => a.Barber).Include(a => a.Customer).Include(a => a.AppointmentStatus).Include(a => a.Services).ThenInclude(e=>e.Service).Select(i => new SaloonReservation.ViewModels.AppointmentVmModel
            {


                BarberId = i.BaberId.Value,
                BarberName = i.Barber.FullName,
                BarberPhone = i.Barber.Phone,
                CustomerName = i.Customer.FullName,
                TotalAmount = i.TotalAmount.Value,
                TotalDuration = i.Services.Sum(e=>e.Service.Duration),
                Day = i.AppointmentCreateDate.Value.DayOfWeek.ToString(),
                AppointmentStartDate = i.AppointmentStartDate.Value.ToShortTimeString(),
                AppointmentEndDate = i.AppointmentEndDate.Value.ToShortTimeString(),
                AppointmentCreateDate = i.AppointmentCreateDate.Value.ToShortDateString(),
                AppointmentCreateDateNotFormat = i.AppointmentCreateDate.Value,
                AppointmentStatus= i.AppointmentStatus.AppointmentStatusTitleEN,

            }).ToList();

            if (filterModel.BarberId != null)
            {
                ds = ds.Where(i => i.BarberId == filterModel.BarberId).ToList();
            }
            if (filterModel.radioBtn != null)
            {
                if (filterModel.radioBtn == "OnDay")
                {
                    if (filterModel.OnDay != null)
                    {
                        ds = ds.Where(i => i.AppointmentCreateDateNotFormat.Date == filterModel.OnDay.Value.Date).ToList();
                    }
                    else
                    {
                        ds = null;
                    }
                }
                else if (filterModel.radioBtn == "Period")
                {
                    if (filterModel.FromDate != null && filterModel.ToDate == null)
                    {
                        ds = null;
                    }
                    if (filterModel.FromDate == null && filterModel.ToDate != null)
                    {
                        ds = null;
                    }
                    if (filterModel.FromDate != null && filterModel.ToDate != null)

                    {
                        ds = ds.Where(i => i.AppointmentCreateDateNotFormat.Date >= filterModel.FromDate.Value.Date && i.AppointmentCreateDateNotFormat <= filterModel.ToDate.Value.Date).ToList();
                    }
                }
            }
            if (filterModel.radioBtn == null && (filterModel.OnDay != null || filterModel.FromDate != null || filterModel.ToDate != null))
            {
                ds = null;
            }

            if (filterModel.BarberId == null && filterModel.FromDate == null && filterModel.ToDate == null && filterModel.radioBtn == null)
            {
                ds = null;
            }

            Report = new AppointmentReportNewt();
            Report.DataSource = ds;
            return Page();

        }

    }
}
