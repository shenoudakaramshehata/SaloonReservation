using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NToastNotify;
using SaloonReservation.Data;
using SaloonReservation.Models;
using SaloonReservation.ViewModels;
using System.Globalization;
using System.Threading;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Localization;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SaloonReservation.Migrations.Salon;
using System.Net;
using System;

namespace SaloonReservation.Pages
{
	public class FastReservationModel : PageModel
	{
		private readonly SignInManager<ApplicationUser> _signInManager;

		private readonly UserManager<ApplicationUser> _userManager;
		public static int AppoimentId { get; set; }

		private SalonContext _context;
		private readonly IToastNotification _toastNotification;
		public IRequestCultureFeature locale;
		public string BrowserCulture;
        public int AppointmentId { get; set; }
        [BindProperty]
		public BillingAAddress billingAAddress { get; set; }
		public TemporaryAppointment TemporaryAppoiment { get; set; }
        public ApplicationUser user { get; set; }
		public FastReservationModel(SalonContext context, IToastNotification toastNotification, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
		{
			_context = context;
			_toastNotification = toastNotification;
			_signInManager = signInManager;
			_userManager = userManager;
			billingAAddress = new BillingAAddress();
		}
		//public void OnGet(int appointmentId)
		public async Task<IActionResult> OnGet(int AppoimentId)
		{
            AppointmentId = AppoimentId;
            TemporaryAppoiment = _context.TemporaryAppointments.Include(e => e.TemporaryAppointmentServices).Where(e => e.TemporaryAppointmentId == AppoimentId).FirstOrDefault();
            if (TemporaryAppoiment == null)
            {
                return Redirect("/AppoimentServices");

            }
             user = await _userManager.GetUserAsync(User);
			if (user != null)
			{
				var customer = _context.Customers.Where(e => e.Email == user.Email).FirstOrDefault();
				if (customer != null)
				{


					billingAAddress.Email = customer.Email;
					billingAAddress.FullName = customer.FullName;
					billingAAddress.FullAddress = customer.FullAddress;
					billingAAddress.Lat = customer.Lat;
					billingAAddress.Lng = customer.Lng;
					billingAAddress.Phone = customer.Phone;
					billingAAddress.CountryId = customer.CountryId;
					billingAAddress.CityId = customer.CityId.Value;
					billingAAddress.AreaId = customer.AreaId.Value;


				}
			}
			return Page();

		}

		public async Task<IActionResult> OnPostAsync(int AppoimentId,string Email, string Password, bool RememberMe)
		{



			var user = await _userManager.FindByEmailAsync(Email);
			if (user == null)
			{
				ModelState.AddModelError(string.Empty, "Invalid login attempt.");
				return Page();
			}
			var result = await _signInManager.PasswordSignInAsync(Email, Password, RememberMe, lockoutOnFailure: false);
			if (result.Succeeded)
			{

				_toastNotification.AddSuccessToastMessage("Login Successfully");
                var appointment = _context.TemporaryAppointments.Where(e => e.TemporaryAppointmentId == AppoimentId).FirstOrDefault();
                if (appointment != null)
                {
					var customer = _context.Customers.Where(e => e.Email == Email).FirstOrDefault();
					if (customer != null)
					{
                        appointment.CustomerId = customer.CustomerId;
                        _context.Attach(appointment).State = EntityState.Modified;
                        _context.SaveChanges();
                    }
                   
                }


            }

            return Redirect($"/FastReservation?AppoimentId={AppoimentId}");
            // If we got this far, something failed, redisplay form
            //return Redirect("/FastReservation");
        }

        public IActionResult OnGetAddCity(int countryId)
		{
			var cities = _context.Cities.Where(e => e.CountryId == countryId).Select(e => new
			{
                CityId  = e.CityId,
                CityTlAr= e.CityTlAr,
                CityTlEn= e.CityTlEn
			}).ToList();
			return new JsonResult(cities);
		}

        public IActionResult OnGetAddArea(int cityId)
        {
        var areas = _context.Areas.Where(e => e.CityId == cityId).Select(e => new
            {
                AreaId = e.AreaId,
                AreaTlAr = e.AreaTlAr,
                AreaTlEn = e.AreaTlEn
        }).ToList();
            return new JsonResult(areas);
        }


		public async Task<IActionResult> OnPostAddCustomer(int AppoimentId)
		{
            var user = await _userManager.GetUserAsync(User);
			var checkPassword = Request.Form["shipAddress"];
			if (user == null)
			{
				if(checkPassword == "on")
				{
                    var userExists = await _userManager.FindByEmailAsync(billingAAddress.Email);
                    if (userExists != null)
                    {
                        _toastNotification.AddErrorToastMessage("Email is already exist");
                        return Redirect($"/FastReservation?AppoimentId={AppoimentId}");
                    }
                    user = new ApplicationUser { UserName = billingAAddress.Email, Email = billingAAddress.Email, FullName = billingAAddress.FullName, PhoneNumber = billingAAddress.Phone };
                    var result = await _userManager.CreateAsync(user, billingAAddress.Password);

                    if (result.Errors.Any())
                    {

                        _toastNotification.AddErrorToastMessage("Something went Error");
                        return Redirect($"/FastReservation?AppoimentId={AppoimentId}");

                    }
                    
                }

				var customer = new Customer()
				{
					FullAddress = billingAAddress.FullAddress,
					CountryId = billingAAddress.CountryId,
					AreaId = billingAAddress.AreaId,
					CityId = billingAAddress.CityId,
					FullName = billingAAddress.FullName,
					Email = billingAAddress.Email,
					Phone = billingAAddress.Phone,
					Lat = billingAAddress.Lat,
					Lng = billingAAddress.Lng,
				};
                _context.Customers.Add(customer);
                _context.SaveChanges();
                var appointment = _context.TemporaryAppointments.Where(e => e.TemporaryAppointmentId == AppoimentId).FirstOrDefault();
                if (appointment != null)
                {
                    appointment.CustomerId = customer.CustomerId;
                    _context.Attach(appointment).State = EntityState.Modified;
                    _context.SaveChanges();
                }

            }
			else
			{
                var customer = _context.Customers.Where(e => e.Email == user.Email).FirstOrDefault();
                if (customer != null)
                {

                    customer.Phone = billingAAddress.Phone;
                    customer.Email = billingAAddress.Email;
                    customer.FullAddress = billingAAddress.FullAddress;
                    customer.CountryId = billingAAddress.CountryId;
                    customer.AreaId = billingAAddress.AreaId;
                    customer.CityId = billingAAddress.CityId;
                    customer.FullName = billingAAddress.FullName;
                    customer.Lat = billingAAddress.Lat;
                    customer.Lng = billingAAddress.Lng;


                    _context.Attach(customer).State = EntityState.Modified;
                    _context.SaveChanges();
                    var appointment = _context.TemporaryAppointments.Where(e => e.TemporaryAppointmentId == AppoimentId).FirstOrDefault();
                    if (appointment != null)
                    {
                        appointment.CustomerId = customer.CustomerId;
                        _context.Attach(appointment).State = EntityState.Modified;
                        _context.SaveChanges();
                    }
                }
                else
                {
                    var Newcustomer = new Customer()
                    {
                        FullAddress = billingAAddress.FullAddress,
                        CountryId = billingAAddress.CountryId,
                        AreaId = billingAAddress.AreaId,
                        CityId = billingAAddress.CityId,
                        FullName = billingAAddress.FullName,
                        Email = billingAAddress.Email,
                        Phone = billingAAddress.Phone,
                        Lat = billingAAddress.Lat,
                        Lng = billingAAddress.Lng,
                    };
                    _context.Customers.Add(Newcustomer);
                    _context.SaveChanges();

                    var appointment = _context.TemporaryAppointments.Where(e => e.TemporaryAppointmentId == AppoimentId).FirstOrDefault();
                    if (appointment != null)
                    {
                        appointment.CustomerId = Newcustomer.CustomerId;
                        _context.Attach(appointment).State = EntityState.Modified;
                        _context.SaveChanges();
                    }
                }

                


            }

            return Redirect($"/PriviewAppoiment?AppoimentId={AppoimentId}");
        }

	}
}

