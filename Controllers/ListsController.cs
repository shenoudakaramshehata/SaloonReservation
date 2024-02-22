using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaloonReservation.Data;
using SaloonReservation.Models;
using SaloonReservation.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using DevExtreme.AspNet.Mvc;
using DevExtreme.AspNet.Data;
using SaloonReservation.Migrations.Salon;

namespace SaloonReservation.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ListsController : Controller
    {
        private SalonContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ListsController(SalonContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        
 [HttpGet]
        public async Task<ActionResult<IEnumerable<contactus>>> GetMessages()
        {
            var data = await _context.contactUs.Select(i => new
            {
                FullName = i.Name,
                Email = i.Email,
                TransDate = i.SendingDate.Value.ToString("MM/dd/yyyy"),
                Msg = i.Message,
                ContactId = i.ContactUsID,

            }).ToListAsync();


            return Ok(new { data });
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SoicialMidiaLink>>> GetSocialLinks()
        {
            var data = await _context.SoicialMidiaLinks.Select(i => new
            {
                Facebook = i.Facebook,
                Instgram = i.Instgram,
                Twitter = i.Twitter,
                WhatsApp = i.WhatsApp,
                LinkedIn = i.LinkedIn,
                Youtube = i.Youtube,
                SocialMediaLinkId = i.SoicialMidiaLinkId,

            }).ToListAsync();


            return Ok(new { data });
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Country>>> GetAllCountries()
        {
            var data = await _context.Countries.OrderBy(e => e.CountryOrderIndex).Select(i => new
            {
                CountryId = i.CountryId,
                CountryTlEn = i.CountryTlEn,
                CountryTlAr = i.CountryTlAr,
                CountryOrderIndex = i.CountryOrderIndex,
                CountryIsActive = i.CountryIsActive,

            }).ToListAsync();


            return Ok(new { data });
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<City>>> GetAllCities()
        {
            var data = await _context.Cities.Include(e => e.Country).OrderBy(e => e.CityOrderIndex).Select(i => new
            {
                CityId = i.CityId,
                CityTlEn = i.CityTlEn,
                CityTlAr = i.CityTlAr,
                CityOrderIndex = i.CityOrderIndex,
                CityIsActive = i.CityIsActive,
                CountryTlAr = i.Country.CountryTlAr,
                CountryTlEn = i.Country.CountryTlEn,


            }).ToListAsync();


            return Ok(new { data });
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<City>>> GetAllAreas()
        {
            var data = await _context.Areas.Include(e => e.City).OrderBy(e => e.AreaOrderIndex).Select(i => new
            {
                AreaId = i.AreaId,
                AreaTlEn = i.AreaTlEn,
                AreaTlAr = i.AreaTlAr,
                AreaOrderIndex = i.AreaOrderIndex,
                AreaIsActive = i.AreaIsActive,
                CityTlAr = i.City.CityTlAr,
                CityTlEn = i.City.CityTlEn,


            }).ToListAsync();


            return Ok(new { data });
        }
        
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAllAppointments()
        {
            var data = await _context.Appointments.Include(e => e.Customer).Include(e => e.Barber).Include(e => e.AppointmentStatus).OrderBy(e => e.AppointmentCreateDate).Select(i => new
            {
                AppointmentId = i.AppointmentId,
                AppointmentCreateDate = (Convert.ToDateTime(i.AppointmentCreateDate.Value.Date.ToShortDateString())).ToString("MMMM d", System.Globalization.CultureInfo.InvariantCulture),
                AppointmentStartDate = i.AppointmentStartDate.Value.ToShortTimeString(),
                AppointmentEndDate = i.AppointmentEndDate.Value.ToShortTimeString(),
                BarberName = i.Barber.FullName,
                CustomerName = i.Customer.FullName,
                AppointmentStatus = i.AppointmentStatus.AppointmentStatusTitleEN,
                TotalAmount = i.TotalAmount,
            }).ToListAsync();


            return Ok(new { data });
        }
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAllBarberAppointments(int BarberId)
        {
           
            var data = await _context.Appointments.Include(e => e.Customer).Include(e => e.Barber).Include(e => e.AppointmentStatus).Where(e => e.BaberId == BarberId && e.AppointmentStatusId == 1).OrderBy(e => e.AppointmentCreateDate).Select(i => new
            {
                AppointmentId = i.AppointmentId,
                AppointmentCreateDate = (Convert.ToDateTime(i.AppointmentCreateDate.Value.Date.ToShortDateString())).ToString("MMMM d", System.Globalization.CultureInfo.InvariantCulture),
                AppointmentStartDate = i.AppointmentStartDate.Value.ToShortTimeString(),
                AppointmentEndDate = i.AppointmentEndDate.Value.ToShortTimeString(),
                BarberName = i.Barber.FullName,
                CustomerName = i.Customer.FullName,
                AppointmentStatus = i.AppointmentStatus.AppointmentStatusTitleEN,
                TotalAmount = i.TotalAmount,
            }).ToListAsync();
            return Ok(new { data });
        }
        [HttpGet]
        public async Task<IActionResult> BarberLookup(DataSourceLoadOptions loadOptions)
        {
            var lookup = from i in _context.Barbers.Where(e=>e.IsActive==true)
                         orderby i.FullName
                         select new
                         {
                             Value = i.BarberId,
                             Text = i.FullName
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }


    }
}