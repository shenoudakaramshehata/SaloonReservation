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

namespace SaloonReservation.Pages
{
	public class AppoimentServicesModel : PageModel
	{
		public List<Service> Services = new List<Service>();
		public List<Barber> Barbers = new List<Barber>();
		public List<Gender> Genders = new List<Gender>();
		public static int AppoimentId { get; set; }
		public  int BaberId { get; set; }
		public int AppoimentNoStaticId { get; set; }
		public static int GenderIdStatic { get; set; }
		private SalonContext _context;
		private readonly IToastNotification _toastNotification;
		public IRequestCultureFeature locale;
		public string BrowserCulture;
		public string BarberName;
		public TemporaryAppointment TemporaryAppoiment { get; set; }
		public string Url { get; set; }
		[BindProperty]
		public AppoimentStatusPageModel appoimentStatusPageModel { get; set; }
		public HttpClient httpClient { get; set; }
		private static bool GenderIsMale = true;
		public AppoimentServicesModel(SalonContext context, IToastNotification toastNotification)
		{
			_context = context;
			_toastNotification = toastNotification;
			appoimentStatusPageModel = new AppoimentStatusPageModel();
		}
		public IActionResult OnGet(int GenderId, int appointmentId, int BarberId)
		{
			TemporaryAppoiment = _context.TemporaryAppointments.Include(e => e.TemporaryAppointmentServices).Where(e => e.TemporaryAppointmentId == appointmentId).FirstOrDefault();
			if (TemporaryAppoiment != null)
			{
				appoimentStatusPageModel.BarberId = TemporaryAppoiment.BaberId;
				AppoimentId = TemporaryAppoiment.TemporaryAppointmentId;
				AppoimentNoStaticId = TemporaryAppoiment.TemporaryAppointmentId;
				appoimentStatusPageModel.AppoimentId = TemporaryAppoiment.TemporaryAppointmentId;

			}
			var barberObj=_context.Barbers.Where(e => e.BarberId == BarberId&&e.IsActive==true).FirstOrDefault();
			if (barberObj == null)
			{
				return Redirect("/Barbers");
			}
			BarberName = barberObj.FullName;

            if (BarberId != 0)
			{
				appoimentStatusPageModel.BarberId = BarberId;
				
			}
			
			if (GenderId == 0)
			{
				GenderId = 1;
			}
			else
			{
				GenderIdStatic = GenderId;
			}
			Url = $"{this.Request.Scheme}://{this.Request.Host}";
			locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
			BrowserCulture = locale.RequestCulture.UICulture.ToString();
			Services = _context.Services.Where(e => e.GenderId == GenderId).ToList();
			appoimentStatusPageModel.GenderId = GenderId;
			Barbers = _context.Barbers.ToList();
			Genders = _context.Genders.ToList();
			return Page();
		}
		public IActionResult OnPost()
		{


			var ServiceList = Request.Form["Services"].ToString();
			List<TemporaryAppointmentService> temporaryAppointmentServices = new List<TemporaryAppointmentService>();


			//List<AdContentValue> adContentValues = new List<AdContentValue>();
			var checkTemporaryApp = _context.TemporaryAppointments.Include(e => e.TemporaryAppointmentServices).Where(e => e.TemporaryAppointmentId == appoimentStatusPageModel.AppoimentId).FirstOrDefault();
			if (checkTemporaryApp != null)
			{
				if (ServiceList != null)
				{
					var values = ServiceList.Split(",");

					for (int i = 0; i < values.Length; i++)
					{
						int result = 0;
						bool check = int.TryParse(values[i], out result);
						if (check)
						{
							var serviceObj = _context.Services.Where(e => e.ServiceId == result).FirstOrDefault();
                            #region Service Old Logic
                            //                     var serviceTempo = _context.TemporaryAppointmentServices.Where(e => e.ServiceId == result && e.TemporaryAppointmentId == checkTemporaryApp.TemporaryAppointmentId).FirstOrDefault();
                            //if (serviceTempo != null)
                            //{
                            //	if (checkTemporaryApp.TemporaryAppointmentServices != null)
                            //	{
                            //		foreach (var item in checkTemporaryApp.TemporaryAppointmentServices)
                            //		{
                            //			var Service = _context.Services.Where(e => e.ServiceId == item.ServiceId).FirstOrDefault();
                            //			var oldServices = _context.TemporaryAppointmentServices.Where(e => e.ServiceId == item.ServiceId && e.TemporaryAppointmentId == checkTemporaryApp.TemporaryAppointmentId).FirstOrDefault();

                            //			if (oldServices != null)
                            //			{
                            //				if (oldServices.TemporaryAppointmentServiceId == serviceTempo.TemporaryAppointmentServiceId)
                            //				{
                            //					oldServices.Amount = oldServices.NumberOfKids * Service.MoreKidsPrice+appoimentStatusPageModel.NumberOfKids * Service.MoreKidsPrice;
                            //					oldServices.NumberOfKids = oldServices.NumberOfKids + appoimentStatusPageModel.NumberOfKids;
                            //                                         _context.Attach(serviceTempo).State = EntityState.Modified;
                            //                                     }
                            //				else
                            //				{
                            //					oldServices.Amount = oldServices.NumberOfKids * Service.MoreKidsPrice;
                            //                                         _context.Attach(serviceTempo).State = EntityState.Modified;

                            //                                     }

                            //			}

                            //		}

                            //	}



                            //	//                        if (appoimentStatusPageModel.NumberOfKids > 1)
                            //	//                        {

                            //	//                            serviceTempo.Amount = serviceTempo.Amount + serviceObj.MoreKidsPrice * appoimentStatusPageModel.NumberOfKids;
                            //	//                            serviceTempo.NumberOfKids = serviceTempo.NumberOfKids+ appoimentStatusPageModel.NumberOfKids;

                            //	//                        }
                            //	//                        else
                            //	//                        {
                            //	//                            serviceTempo.Amount = serviceTempo.Amount + serviceObj.OneKidPrice;
                            //	//	serviceTempo.NumberOfKids = serviceTempo.NumberOfKids + appoimentStatusPageModel.NumberOfKids;

                            //	//}
                            //	//                        _context.Attach(serviceTempo).State = EntityState.Modified;

                            //}
                            //else
                            //{
                            //	if (checkTemporaryApp.TemporaryAppointmentServices != null)
                            //	{
                            //		foreach (var item in checkTemporaryApp.TemporaryAppointmentServices)
                            //		{
                            //			var Service = _context.Services.Where(e => e.ServiceId == item.ServiceId).FirstOrDefault();
                            //			var oldServices = _context.TemporaryAppointmentServices.Where(e => e.ServiceId == item.ServiceId && e.TemporaryAppointmentId == checkTemporaryApp.TemporaryAppointmentId).FirstOrDefault();

                            //			if (oldServices != null)
                            //			{

                            //				oldServices.Amount = oldServices.NumberOfKids * Service.MoreKidsPrice;
                            //                                     _context.Attach(oldServices).State = EntityState.Modified;
                            //                                 }

                            //		}
                            //		var TemporayAppService = new TemporaryAppointmentService()
                            //		{
                            //			Amount = serviceObj.MoreKidsPrice * appoimentStatusPageModel.NumberOfKids,
                            //			GenderId = serviceObj.GenderId,
                            //			ServiceId = serviceObj.ServiceId,
                            //			NumberOfKids = appoimentStatusPageModel.NumberOfKids,
                            //			TemporaryAppointmentId = checkTemporaryApp.TemporaryAppointmentId,

                            //		};
                            //                             _context.TemporaryAppointmentServices.AddRange(TemporayAppService);

                            //                            // temporaryAppointmentServices.Add(TemporayAppService);

                            //		//if (appoimentStatusPageModel.NumberOfKids > 1)
                            //		//{
                            //		//    var TemporayAppService = new TemporaryAppointmentService()
                            //		//    {
                            //		//        Amount = serviceObj.MoreKidsPrice * appoimentStatusPageModel.NumberOfKids,
                            //		//        GenderId = serviceObj.GenderId,
                            //		//        ServiceId = serviceObj.ServiceId,
                            //		//        TemporaryAppointmentId = checkTemporaryApp.TemporaryAppointmentId,

                            //		//    };
                            //		//    temporaryAppointmentServices.Add(TemporayAppService);

                            //		//}
                            //		//else
                            //		//{
                            //		//    var TemporayAppService = new TemporaryAppointmentService()
                            //		//    {
                            //		//        Amount = serviceObj.OneKidPrice,
                            //		//        GenderId = serviceObj.GenderId,
                            //		//        ServiceId = serviceObj.ServiceId,
                            //		//        TemporaryAppointmentId = checkTemporaryApp.TemporaryAppointmentId,

                            //		//    };
                            //		//    temporaryAppointmentServices.Add(TemporayAppService);

                            //		//}
                            //		//_context.TemporaryAppointmentServices.AddRange(temporaryAppointmentServices);
                            //	}

                            //}
                            #endregion
                           
                            
                                if (checkTemporaryApp.TemporaryAppointmentServices != null)
                                {
                                    foreach (var item in checkTemporaryApp.TemporaryAppointmentServices)
                                    {
                                        var Service = _context.Services.Where(e => e.ServiceId == item.ServiceId).FirstOrDefault();
                                        var oldServices = _context.TemporaryAppointmentServices.Where(e => e.ServiceId == item.ServiceId && e.TemporaryAppointmentId == checkTemporaryApp.TemporaryAppointmentId).FirstOrDefault();

                                        if (oldServices != null)
                                        {

                                            oldServices.Amount = oldServices.NumberOfKids * Service.MoreKidsPrice;
                                            _context.Attach(oldServices).State = EntityState.Modified;
                                        }

                                    }
                                    var TemporayAppService = new TemporaryAppointmentService()
                                    {
                                        Amount = serviceObj.MoreKidsPrice * appoimentStatusPageModel.NumberOfKids,
                                        GenderId = serviceObj.GenderId,
                                        ServiceId = serviceObj.ServiceId,
                                        NumberOfKids = appoimentStatusPageModel.NumberOfKids,
                                        TemporaryAppointmentId = checkTemporaryApp.TemporaryAppointmentId,

                                    };
                                    _context.TemporaryAppointmentServices.AddRange(TemporayAppService);

                                   
                                }

                           
                        }

                    }

					checkTemporaryApp.BaberId = appoimentStatusPageModel.BarberId;
					checkTemporaryApp.TotalAmount = checkTemporaryApp.TemporaryAppointmentServices.Sum(e => e.Amount);
					_context.Attach(checkTemporaryApp).State = EntityState.Modified;

					_context.SaveChanges();


				}

			}
			else
			{
				if (ServiceList != null)
				{
					var values = ServiceList.Split(",");

					for (int i = 0; i < values.Length; i++)
					{
						int result = 0;
						bool check = int.TryParse(values[i], out result);
						if (check)
						{
							var serviceObj = _context.Services.Where(e => e.ServiceId == result).FirstOrDefault();
							if (appoimentStatusPageModel.NumberOfKids > 1)
							{
								var TemporayAppService = new TemporaryAppointmentService()
								{
									Amount = serviceObj.MoreKidsPrice * appoimentStatusPageModel.NumberOfKids,
									NumberOfKids = appoimentStatusPageModel.NumberOfKids,
									GenderId = serviceObj.GenderId,
									ServiceId = serviceObj.ServiceId,

								};
								temporaryAppointmentServices.Add(TemporayAppService);

							}
							else
							{
								var TemporayAppService = new TemporaryAppointmentService()
								{
									Amount = serviceObj.OneKidPrice,
									GenderId = serviceObj.GenderId,
									NumberOfKids = appoimentStatusPageModel.NumberOfKids,
									ServiceId = serviceObj.ServiceId,

								};
								temporaryAppointmentServices.Add(TemporayAppService);

							}
						}



					}

					var TemporaryAppointmentObj = new TemporaryAppointment()
					{
						PaymentMethodId = 1,
						TotalAmount = temporaryAppointmentServices.Sum(e => e.Amount),
						BaberId = appoimentStatusPageModel.BarberId,
						CustomerId = null,
						IsPaid = false,
						Remarks = "",
						AppointmentStatusId = 1,
						AppointmentCreateDate = DateTime.Now,
						AppointmentStartDate = DateTime.Now,
						AppointmentEndDate = DateTime.Now,
						TemporaryAppointmentServices = temporaryAppointmentServices,

					};

					_context.TemporaryAppointments.Add(TemporaryAppointmentObj);
					_context.SaveChanges();
					AppoimentId = TemporaryAppointmentObj.TemporaryAppointmentId;
					

				}
			}
			BaberId = appoimentStatusPageModel.BarberId;
			return Redirect($"/AppoimentServices?GenderId={GenderIdStatic}&&appointmentId={AppoimentId}&&BarberId={BaberId}");
		}



	}
}
