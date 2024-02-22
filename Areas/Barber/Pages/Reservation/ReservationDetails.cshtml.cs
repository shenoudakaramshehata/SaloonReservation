using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using SaloonReservation.Data;
using SaloonReservation.Models;
using SaloonReservation.Services;
using SaloonReservation.ViewModels;

namespace SaloonReservation.Areas.Barber.Pages.Reservation
{
    public class ReservationDetailsModel : PageModel
    {
        private readonly SalonContext _context;
        public static int StaticAppointmentId { get; set; }
        public TemporaryAppointment temporaryAppointment { set; get; }
        private readonly IToastNotification _toastNotification;
        public AppointmentVmModel AppointmentVmModel { get; set; }

        public ReservationDetailsModel(SalonContext context, IEmailSender emailSender, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;

        }


        public async Task<IActionResult> OnGet(int AppointmentId)
        {
            var appoiment = _context.Appointments.Include(e => e.Customer).Include(e => e.Barber).Include(e => e.Services).Include(e => e.AppointmentStatus).Where(e => e.AppointmentId == AppointmentId).FirstOrDefault();
            if (appoiment == null)
            {
                return Page();
            }
            StaticAppointmentId = AppointmentId;
            AppointmentVmModel = new AppointmentVmModel()
            {
                AppointmentId = appoiment.AppointmentId,
                Day = appoiment.AppointmentCreateDate.Value.DayOfWeek.ToString(),
                AppointmentStartDate = appoiment.AppointmentStartDate.Value.ToShortTimeString(),
                AppointmentEndDate = appoiment.AppointmentEndDate.Value.ToShortTimeString(),
                TotalAmount = appoiment.TotalAmount.Value,
                PaymentMethod = "My Fattorah",
                PaymentId = appoiment.FattorahPaymentId,

            };
            DateTime dateTime = Convert.ToDateTime(appoiment.AppointmentCreateDate.Value.Date.ToShortDateString());
            AppointmentVmModel.AppointmentCreateDate = dateTime.ToString("MMMM d", System.Globalization.CultureInfo.InvariantCulture);

            if (appoiment.Barber != null)
            {
                AppointmentVmModel.BarberName = appoiment.Barber.FullName;
                AppointmentVmModel.BarberPhone = appoiment.Barber.Phone;
                AppointmentVmModel.BarberImage = appoiment.Barber.Image;
                AppointmentVmModel.BarberEmail = appoiment.Barber.Email;

            }
            if (appoiment.AppointmentStatus != null)
            {
                AppointmentVmModel.AppointmentStatus = appoiment.AppointmentStatus.AppointmentStatusTitleEN;


            }
            double totalDuration = 0;
            //var services = _context.AppointmentServices.Where(e => e.AppointmentId == appoiment.AppointmentId).ToList();
            if (appoiment.Services != null)
            {
                List<AppointmentServiceVM> AppointmentServiceVMList = new List<AppointmentServiceVM>();
                foreach (var item in appoiment.Services)
                {
                    var serviceObj = _context.Services.Where(e => e.ServiceId == item.ServiceId).FirstOrDefault();
                    totalDuration += item.NumberOfKids * serviceObj.Duration;
                    var gender = _context.Genders.Where(e => e.GenderId == item.GenderId).FirstOrDefault();
                    var appService = new AppointmentServiceVM()
                    {
                        Gender = gender.GenderTLEn,
                        //NumberOfKids = 1,
                        ServiceDuration = item.NumberOfKids * serviceObj.Duration,
                        Amount = item.Amount.Value,
                        ServiceTitle = serviceObj.serviceTlEn,
                        ServiceImage = "/Images/PublicSlider/c4498a45-1789-4638-8ea2-9b41a0e35ea3_slide-1.jpg",

                    };
                    AppointmentServiceVMList.Add(appService);

                }
                AppointmentVmModel.TotalDuration = totalDuration;
                AppointmentVmModel.Services = AppointmentServiceVMList;
            }
            if (appoiment.Customer != null)
            {
                var country = _context.Countries.Where(e => e.CountryId == appoiment.Customer.CountryId).FirstOrDefault().CountryTlAr;
                var City = _context.Cities.Where(e => e.CityId == appoiment.Customer.CountryId).FirstOrDefault().CityTlAr;
                var Area = _context.Areas.Where(e => e.AreaId == appoiment.Customer.AreaId).FirstOrDefault().AreaTlAr;
                AppointmentVmModel.CustomerCountry = country;
                AppointmentVmModel.CustomerCity = City;
                AppointmentVmModel.CustomerArea = Area;
                AppointmentVmModel.CustomerAddress = appoiment.Customer.FullAddress;
                AppointmentVmModel.CustomerPhone = appoiment.Customer.Phone;
                AppointmentVmModel.CustomerEmail = appoiment.Customer.Email;
                AppointmentVmModel.CustomerName = appoiment.Customer.FullName;
                AppointmentVmModel.CustomerLat = appoiment.Customer.Lat;
                AppointmentVmModel.CustomerLng = appoiment.Customer.Lng;
            }
            
            return Page();


        }
        
        public async Task<IActionResult> OnPostCancelReservation(string CancleRemarks)
        {
            try
            {
                var model = _context.Appointments.Where(c => c.AppointmentId == StaticAppointmentId).FirstOrDefault();
                if (model == null)
                {
                    return Redirect($"/Barber/Reservation/ReservationDetails?AppointmentId={StaticAppointmentId}");
                }
                model.AppointmentStatusId =3;
                model.Remarks = CancleRemarks;
                var UpdatedAppointment = _context.Appointments.Attach(model);
                UpdatedAppointment.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Reservation Canceled Successfully");
            }
            catch (Exception e)
            {
                _toastNotification.AddErrorToastMessage("Something went Error");
            }
            //return Redirect($"/Barber/Reservation/ReservationDetails?AppointmentId={StaticAppointmentId}");
            return Redirect($"/Barber/Reservation/Index");

        }
        public async Task<IActionResult> OnPostCloseReservation()
        {
            try
            {
                var model = _context.Appointments.Where(c => c.AppointmentId == StaticAppointmentId).FirstOrDefault();
                if (model == null)
                {
                    return Redirect($"/Barber/Reservation/ReservationDetails?AppointmentId={StaticAppointmentId}");
                }
                model.AppointmentStatusId = 2;
                var UpdatedAppointment = _context.Appointments.Attach(model);
                UpdatedAppointment.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
                _toastNotification.AddSuccessToastMessage("Reservation Closed Successfully");
            }
            catch (Exception e)
            {
                _toastNotification.AddErrorToastMessage("Something went Error");
            }
            //return Redirect($"/Barber/Reservation/ReservationDetails?AppointmentId={StaticAppointmentId}");
            return Redirect($"/Barber/Reservation/Index");

        }
    }
}
