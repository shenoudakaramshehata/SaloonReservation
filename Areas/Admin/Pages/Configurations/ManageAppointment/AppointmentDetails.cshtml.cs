using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SaloonReservation.Data;
using SaloonReservation.Models;
using SaloonReservation.Services;
using SaloonReservation.ViewModels;

namespace SaloonReservation.Areas.Admin.Pages.Configurations.ManageAppointment
{
    public class AppointmentDetailsModel : PageModel
    {
        private readonly SalonContext _context;
        public TemporaryAppointment temporaryAppointment { set; get; }

        public AppointmentVmModel AppointmentVmModel { get; set; }

        public AppointmentDetailsModel(SalonContext context, IEmailSender emailSender)
        {
            _context = context;

        }


        public async Task<IActionResult> OnGet(int AppointmentId)
        {
            var appoiment = _context.Appointments.Include(e => e.Customer).Include(e => e.Barber).Include(e => e.Services).Include(e => e.AppointmentStatus).Where(e => e.AppointmentId == AppointmentId).FirstOrDefault();
            if (appoiment == null)
            {
                return Page();
            }
            AppointmentVmModel = new AppointmentVmModel()
            {
                Day = appoiment.AppointmentCreateDate.Value.DayOfWeek.ToString(),
                AppointmentStartDate = appoiment.AppointmentStartDate.Value.ToShortTimeString(),
                AppointmentEndDate = appoiment.AppointmentEndDate.Value.ToShortTimeString(),
               TotalAmount= appoiment.TotalAmount.Value,
               PaymentMethod="My Fattorah",
               PaymentId= appoiment.FattorahPaymentId,
              
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
            if (appoiment.Services!= null)
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
                        ServiceDuration = item.NumberOfKids*serviceObj.Duration,
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
            //else
            //{
            //    double totalDuration = 0;
            //    if (appoiment.Services != null)
            //    {
            //        foreach (var item in appoiment.Services)
            //        {
            //            var serviceObj = _context.Services.Where(e => e.ServiceId == item.ServiceId).FirstOrDefault();
            //            totalDuration += item.NumberOfKids * serviceObj.Duration;
            //        }
            //    }
            //    if (appoiment.Customer != null)
            //    {
            //        var country = _context.Countries.Where(e => e.CountryId == appoiment.Customer.CountryId).FirstOrDefault().CountryTlAr;
            //        var City = _context.Cities.Where(e => e.CityId == appoiment.Customer.CountryId).FirstOrDefault().CityTlAr;
            //        var Area = _context.Areas.Where(e => e.AreaId == appoiment.Customer.AreaId).FirstOrDefault().AreaTlAr;
            //    }

            //    AppointmentVmModel = new AppointmentVmModel()
            //    {
            //        Day = appoiment.AppointmentCreateDate.Value.DayOfWeek.ToString(),
            //        AppointmentStartDate = appoiment.AppointmentStartDate.Value.ToShortTimeString(),
            //        AppointmentEndDate = appoiment.AppointmentEndDate.Value.ToShortTimeString(),
            //        CustomerName = customer.FullName,
            //        //CustomerAddress ="Country : "+ country.CountryTlEn+ "City : "+City.CityTlEn+ "Area :"+ Area.AreaTlEn + " With Adrees "+customer.FullAddress,
            //        CustomerAddress = customer.FullAddress,
            //        CustomerPhone = customer.Phone,
            //        BarberPhone = barber.Phone,
            //        BarberName = barber.FullName,
            //        TotalAmount = appoiment.TotalAmount.Value,
            //        PaymentMethod = "Mt Fattorah",
            //        AppointmentStatus = status.AppointmentStatusTitleEN,
            //        SiteEmail = "Shenouda20@gmail.com",
            //        SitePhone = "9657482",
            //        TotalDuration = totalDuration

            //    };
            //    DateTime dateTime = Convert.ToDateTime(appoiment.AppointmentCreateDate.Value.Date.ToShortDateString());
            //    AppointmentVmModel.AppointmentCreateDate = dateTime.ToString("MMMM d", System.Globalization.CultureInfo.InvariantCulture);
            //    if (AppointmentVmModel != null)
            //    {
            //        List<AppointmentServiceVM> AppointmentServiceVMList = new List<AppointmentServiceVM>();
            //        foreach (var item in appointmentServices)
            //        {
            //            var gender = _context.Genders.Where(e => e.GenderId == item.GenderId).FirstOrDefault();
            //            var service = _context.Services.Where(e => e.ServiceId == item.ServiceId).FirstOrDefault();
            //            var appService = new AppointmentServiceVM()
            //            {
            //                Gender = gender.GenderTLEn,
            //                NumberOfKids = item.NumberOfKids,
            //                Amount = item.Amount.Value,
            //                ServiceTitle = service.serviceTlEn,
            //                ServiceImage = "/Images/PublicSlider/c4498a45-1789-4638-8ea2-9b41a0e35ea3_slide-1.jpg",

            //            };
            //            AppointmentServiceVMList.Add(appService);
            //        }
            //        AppointmentVmModel.Services = AppointmentServiceVMList;
            //    }

            //}
            return Page();


        }
    }
}
