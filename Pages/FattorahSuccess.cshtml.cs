using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using SaloonReservation.Data;
using SaloonReservation.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.IO;
using SaloonReservation.Services;
using Email;
using Microsoft.AspNetCore.Hosting;
using System;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using SaloonReservation.ViewModels;

namespace SaloonReservation.Pages
{
	public class FattorahSuccessModel : PageModel
	{
		private readonly SalonContext _context;
		public TemporaryAppointment temporaryAppointment { set; get; }
		private readonly IConfiguration _configuration;
		FattorhResult FattoraResStatus { set; get; }
		private readonly IEmailSender _emailSender;
		private IHostingEnvironment _env;
        public AppointmentVmModel AppointmentVmModel { get; set; }
        string res { set; get; }
		private readonly IRazorPartialToStringRenderer _renderer;
		public FattorahSuccessModel(IRazorPartialToStringRenderer renderer,SalonContext context, IEmailSender emailSender,IHostingEnvironment env, IConfiguration configuration)
		{
			_context = context;
			_configuration = configuration;
			_emailSender = emailSender;
			_env = env;
			_renderer = renderer;
		}
		public FattorahPaymentResult fattorahPaymentResult { get; set; }
		static string testURL = "https://apitest.myfatoorah.com/v2/GetPaymentStatus";
		static string liveURL = "https://api.myfatoorah.com/v2/GetPaymentStatus";
		public async Task<IActionResult> OnGet(string paymentId)
		{
			if (paymentId != null)
			{
				var GetPaymentStatusRequest = new
				{
					Key = paymentId,
					KeyType = "paymentId"
				};
				bool Fattorahstatus = bool.Parse(_configuration["FattorahStatus"]);
				var TestToken = _configuration["TestToken"];
				var LiveToken = _configuration["LiveToken"];

				var GetPaymentStatusRequestJSON = JsonConvert.SerializeObject(GetPaymentStatusRequest);

				HttpClient client = new HttpClient();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				if (Fattorahstatus) // fattorah live
				{
					client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", LiveToken);
					var httpContent = new StringContent(GetPaymentStatusRequestJSON, System.Text.Encoding.UTF8, "application/json");
					var responseMessage = client.PostAsync(liveURL, httpContent);
					res = await responseMessage.Result.Content.ReadAsStringAsync();
					FattoraResStatus = JsonConvert.DeserializeObject<FattorhResult>(res);
				}
				else                 // fattorah test
				{
					client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TestToken);
					var httpContent = new StringContent(GetPaymentStatusRequestJSON, System.Text.Encoding.UTF8, "application/json");
					var responseMessage = client.PostAsync(testURL, httpContent);
					res = await responseMessage.Result.Content.ReadAsStringAsync();
					FattoraResStatus = JsonConvert.DeserializeObject<FattorhResult>(res);
				}

				if (FattoraResStatus.IsSuccess == true)
				{
					Newtonsoft.Json.Linq.JObject jObject = Newtonsoft.Json.Linq.JObject.Parse(res);
					fattorahPaymentResult = jObject["Data"].ToObject<FattorahPaymentResult>();
					int TemporaryAppointmentId = 0;
					bool checkRes = int.TryParse(fattorahPaymentResult.UserDefinedField, out TemporaryAppointmentId);
					if (fattorahPaymentResult.InvoiceStatus == "Paid")
					{
						try
						{
							if (fattorahPaymentResult.UserDefinedField != null)
							{

								if (checkRes)
								{

									temporaryAppointment = _context.TemporaryAppointments.Where(e => e.TemporaryAppointmentId == TemporaryAppointmentId).FirstOrDefault();
									var temporaryAppointmentServices = _context.TemporaryAppointmentServices.Where(e => e.TemporaryAppointmentId == TemporaryAppointmentId).ToList();
									//temporaryAppointment.IsPaid = true;
									//temporaryAppointment.PaymentID = paymentId;
									List<AppointmentService> appointmentServices = new List<AppointmentService>();
									foreach (var item in temporaryAppointmentServices)
									{
										var AppoimentServiceObj = new AppointmentService()
										{
											Amount = item.Amount,
											GenderId = item.GenderId,
											ServiceId = item.ServiceId,
                                            NumberOfKids = item.NumberOfKids,
										};
										appointmentServices.Add(AppoimentServiceObj);
									}
									var appoiment = new Appointment()
									{
										AppointmentCreateDate = temporaryAppointment.AppointmentCreateDate,
										AppointmentStartDate = temporaryAppointment.AppointmentStartDate,
										AppointmentEndDate = temporaryAppointment.AppointmentEndDate,
										AppointmentStatusId = temporaryAppointment.AppointmentStatusId,
										BaberId = temporaryAppointment.BaberId,
										CustomerId = temporaryAppointment.CustomerId.Value,
										TotalAmount = temporaryAppointment.TotalAmount,
										Remarks = temporaryAppointment.Remarks,
										FattorahPaymentId = paymentId,
										Services = appointmentServices
									};
									_context.Appointments.Add(appoiment);
									_context.SaveChanges();
									var customer = _context.Customers.Where(e => e.CustomerId == appoiment.CustomerId).FirstOrDefault();
									var barber = _context.Barbers.Where(e => e.BarberId == appoiment.BaberId).FirstOrDefault();
									var status = _context.AppointmentStatuses.Where(e => e.AppointmentStatusId == appoiment.AppointmentStatusId).FirstOrDefault();
									double totalDuration = 0;
									
									AppointmentVmModel = new AppointmentVmModel()
									{
										Day = appoiment.AppointmentCreateDate.Value.DayOfWeek.ToString(),

										AppointmentStartDate = appoiment.AppointmentStartDate.Value.ToShortTimeString(),
										AppointmentEndDate = appoiment.AppointmentEndDate.Value.ToShortTimeString(),
										CustomerName = customer.FullName,
										CustomerAddress = customer.FullAddress,
										CustomerPhone = customer.Phone,
										BarberPhone = barber.Phone,
										BarberName = barber.FullName,
										TotalAmount = appoiment.TotalAmount.Value,
										PaymentMethod = "Mt Fattorah",
										AppointmentStatus = status.AppointmentStatusTitleEN,
										SiteEmail = "Shenouda20@gmail.com",
										SitePhone = "9657482",
									};
                                    DateTime dateTime = Convert.ToDateTime(appoiment.AppointmentCreateDate.Value.Date.ToShortDateString());
                                    AppointmentVmModel.AppointmentCreateDate = dateTime.ToString("MMMM d", System.Globalization.CultureInfo.InvariantCulture);

                                    if (AppointmentVmModel != null)
									{
										List<AppointmentServiceVM> AppointmentServiceVMList = new List<AppointmentServiceVM>();
										foreach (var item in appointmentServices)
										{
                                            
                                            var gender  = _context.Genders.Where(e => e.GenderId ==item.GenderId).FirstOrDefault();
											var service = _context.Services.Where(e => e.ServiceId == item.ServiceId).FirstOrDefault();
                                            totalDuration += item.NumberOfKids * service.Duration;
                                            var appService = new AppointmentServiceVM()
											{
												Gender=gender.GenderTLEn,
												NumberOfKids=item.NumberOfKids,
												Amount=item.Amount.Value,
												ServiceTitle=service.serviceTlEn,
                                                ServiceImage = "/Images/PublicSlider/c4498a45-1789-4638-8ea2-9b41a0e35ea3_slide-1.jpg",


                                            };
											AppointmentServiceVMList.Add(appService);
										}
										AppointmentVmModel.Services = AppointmentServiceVMList;
									}
									AppointmentVmModel.TotalDuration = totalDuration;
                                    
                                   // string email = "shenoudakaram32@gmail.com";
									var messageBody = await _renderer.RenderPartialToStringAsync("_Invoice", AppointmentVmModel);
									//await _emailSender.SendEmailAsync(email, "Appoiment Details", messageBody);
									await _emailSender.SendEmailAsync(customer.Email, "Appoiment Details", messageBody);

									await _emailSender.SendEmailAsync(barber.Email, "Appoiment Details", messageBody);

									return Page();

								}
								return RedirectToPage("SomethingwentError");

							}
						}
						catch (Exception)
						{
							return RedirectToPage("SomethingwentError");
						}


					}
					else
					{
						try
						{
							if (fattorahPaymentResult.UserDefinedField != null)
							{
								if (checkRes)
								{
									///logic

									return Page();
								}
								return RedirectToPage("SomethingwentError");
							}
						}

						catch (Exception)
						{
							return RedirectToPage("SomethingwentError");
						}
					}

				}

			}

			return RedirectToPage("SomethingwentError");
		}

	}
}

