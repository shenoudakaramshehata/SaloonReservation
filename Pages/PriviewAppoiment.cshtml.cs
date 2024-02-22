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
using Humanizer;
using System.Timers;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System;
using System.Net.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Identity;

namespace SaloonReservation.Pages
{
    public class PriviewAppoimentModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public int AppointmentId { get; set; }
        public string AppoimentDate { get; set; }
        public TemporaryAppointment TemporaryAppoiment { get; set; }
        private SalonContext _context;
        private readonly IToastNotification _toastNotification;
		private readonly IConfiguration _configuration;
		private readonly IWebHostEnvironment _hostEnvironment;
		public IRequestCultureFeature locale;
		public string BrowserCulture;
		public string  BarberName { get; set; }
        public HttpClient httpClient { get; set; }
		public PriviewAppoimentModel(SalonContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment hostEnvironment, IConfiguration configuration, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
			_hostEnvironment = hostEnvironment;
			_configuration = configuration;
			httpClient = new HttpClient();
            _userManager = userManager;

        }
        public async Task<IActionResult> OnGetAsync(int AppoimentId)
        {
            AppointmentId = AppoimentId;
            TemporaryAppoiment = _context.TemporaryAppointments.Include(e => e.TemporaryAppointmentServices).Where(e => e.TemporaryAppointmentId == AppoimentId).FirstOrDefault();
            DateTime dateTime = Convert.ToDateTime(TemporaryAppoiment.AppointmentCreateDate.Date.ToShortDateString());
			if (TemporaryAppoiment == null)
			{
				return Redirect($"/Schedule?AppoimentId={AppointmentId}");

			}
            //var user = _context.Customers.Where(a => a.id;
			//if(TemporaryAppoiment.CustomerId is null)
			//{
			//	TemporaryAppoiment.CustomerId = user.Id;

			//}
            var barber = _context.Barbers.Where(e => e.BarberId == TemporaryAppoiment.BaberId).FirstOrDefault();

			if (barber == null)
			{
				return Redirect($"/Schedule?AppoimentId={AppointmentId}");
			}
			BarberName = barber.FullName;

			//// Convert to "August 23"
			AppoimentDate = dateTime.ToString("MMMM d", System.Globalization.CultureInfo.InvariantCulture);
			return Page();

        }
		public async Task<IActionResult> OnPost(int AppoimentId)
		{
			try
			{
				var temporaryAppoiment = _context.TemporaryAppointments.Where(e => e.TemporaryAppointmentId == AppoimentId).FirstOrDefault();
				if (temporaryAppoiment == null)
				{
					return Redirect($"/PriviewAppoiment?AppoimentId={AppoimentId}");
				}
				var Customer = _context.Customers.Where(e => e.CustomerId == temporaryAppoiment.CustomerId).FirstOrDefault();

				if (Customer == null)
				{
					return Redirect($"/PriviewAppoiment?AppoimentId={AppoimentId}");
				}
				
				if (temporaryAppoiment!=null)
				{
					bool Fattorahstatus = bool.Parse(_configuration["FattorahStatus"]);
					var TestToken = _configuration["TestToken"];
					var LiveToken = _configuration["LiveToken"];

					if (Fattorahstatus) // fattorah live
					{
						var sendPaymentRequest = new
						{

							CustomerName = Customer.FullName,
							NotificationOption = "LNK",
							InvoiceValue = temporaryAppoiment.TotalAmount,
							CallBackUrl = "http://codewarenet-001-site13.dtempurl.com/FattorahSuccess",
							ErrorUrl = "http://codewarenet-001-site13.dtempurl.com/FattorahFailed",
							UserDefinedField = temporaryAppoiment.TemporaryAppointmentId,
							CustomerEmail = Customer.Email
						};
						var sendPaymentRequestJSON = JsonConvert.SerializeObject(sendPaymentRequest);

						string url = "https://api.myfatoorah.com/v2/SendPayment";
						httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
						httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", LiveToken);
						var httpContent = new StringContent(sendPaymentRequestJSON, Encoding.UTF8, "application/json");
						var responseMessage = httpClient.PostAsync(url, httpContent);
						var res = await responseMessage.Result.Content.ReadAsStringAsync();
						var FattoraRes = JsonConvert.DeserializeObject<FattorhResult>(res);


						if (FattoraRes.IsSuccess == true)
						{
							Newtonsoft.Json.Linq.JObject jObject = Newtonsoft.Json.Linq.JObject.Parse(res);
							var InvoiceRes = jObject["Data"].ToObject<InvoiceData>();
							return Redirect(InvoiceRes.InvoiceURL);


						}
						else
						{

							_context.TemporaryAppointments.Remove(temporaryAppoiment);
							_toastNotification.AddErrorToastMessage("Something Went Error Please Try Again");
							RedirectToPage("SomethingwentError");



						}
					}
					else               //fattorah test
					{

						var sendPaymentRequest = new
						{

							CustomerName = Customer.FullName,
							NotificationOption = "LNK",
							InvoiceValue = temporaryAppoiment.TotalAmount,
							CallBackUrl = "http://codewarenet-001-site13.dtempurl.com/FattorahSuccess",
							ErrorUrl = "http://codewarenet-001-site13.dtempurl.com/FattorahFailed",
							UserDefinedField = temporaryAppoiment.TemporaryAppointmentId,
							CustomerEmail = Customer.Email
						};
						var sendPaymentRequestJSON = JsonConvert.SerializeObject(sendPaymentRequest);

						string url = "https://apitest.myfatoorah.com/v2/SendPayment";
						httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
						httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TestToken);
						var httpContent = new StringContent(sendPaymentRequestJSON, Encoding.UTF8, "application/json");
						var responseMessage = httpClient.PostAsync(url, httpContent);
						var res = await responseMessage.Result.Content.ReadAsStringAsync();
						var FattoraRes = JsonConvert.DeserializeObject<FattorhResult>(res);


						if (FattoraRes.IsSuccess == true)
						{
							Newtonsoft.Json.Linq.JObject jObject = Newtonsoft.Json.Linq.JObject.Parse(res);
							var InvoiceRes = jObject["Data"].ToObject<InvoiceData>();
							return Redirect(InvoiceRes.InvoiceURL);



						}
						else
						{

							_context.TemporaryAppointments.Remove(temporaryAppoiment);
							_toastNotification.AddErrorToastMessage("Something Went Error Please Try Again");
							RedirectToPage("SomethingwentError");

						}
					}
				}
				
			}
			catch (Exception e)
			{
				return RedirectToPage("SomethingwentError");
			}
			return Page();
		}
	}
}
