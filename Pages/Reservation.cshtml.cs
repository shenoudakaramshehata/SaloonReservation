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

namespace SaloonReservation.Pages
{
    public class ReservationModel : PageModel
    {
        List<AppResultVm> MoreAppo = new List<AppResultVm>();
        public List<DisplaydaySlots> dayTimeSLot = new List<DisplaydaySlots>();
        private SalonContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IToastNotification _toastNotification;
        private readonly IConfiguration _configuration;
        public HttpClient httpClient { get; set; }
        private int count = 0;
        public ReservationModel(SalonContext context, IWebHostEnvironment hostEnvironment, IConfiguration configuration,
                                           IToastNotification toastNotification)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _toastNotification = toastNotification;
            _configuration = configuration;
            httpClient = new HttpClient();

        }
        public void OnGet()
        {
            //List<int> service = new List<int>()
            //{
            //    2,1
            //};
            //GetAppointment(service, DateTime.Now, 1);
            //var MoreAppoments = GetAppointment(service, DateTime.Now, 1);
            //var MoreAppomentsList = MoreAppoments.GroupBy(e => e.Date.Date).ToList();
            
            //dayTimeSLot = new List<DisplaydaySlots>();
            //foreach (var appointmentsForDate in MoreAppomentsList)
            //{
            //    var dayTimeSLotobj = new DisplaydaySlots();
            //    List<SlotsList> SlotsList = new List<SlotsList>();
            //    foreach (var item in appointmentsForDate)
            //    {
            //        string fromTimeString = item.From.TimeOfDay.ToString();
            //        string ToTimeString = item.To.TimeOfDay.ToString();
            //        // Parse the time string to a DateTime
            //        DateTime time = DateTime.ParseExact(fromTimeString, "HH:mm:ss", CultureInfo.InvariantCulture);

            //        // Convert to the 12-hour format with AM/PM
            //        string Form = time.ToString("h:mm tt", CultureInfo.InvariantCulture);

            //        DateTime timeTo = DateTime.ParseExact(ToTimeString, "HH:mm:ss", CultureInfo.InvariantCulture);

            //        // Convert to the 12-hour format with AM/PM
            //        string To = timeTo.ToString("h:mm tt", CultureInfo.InvariantCulture);
            //        var slotlistobj = new SlotsList()
            //        {
            //            From = Form,
            //            To = To,
            //        };
            //        SlotsList.Add(slotlistobj);
            //    }
            //    dayTimeSLotobj.Date = appointmentsForDate.Key;
            //    dayTimeSLotobj.slotsLists = SlotsList;

            //    dayTimeSLot.Add(dayTimeSLotobj);
            //}

        }



        public List<AppResultVm> GetAppointment(List<int> serviceDurations, DateTime day, int BarberId)
        {

            //bool IsReturn = true;
            //var day = DateTime.Now;

            int totalDuration = serviceDurations.Sum();

            do
            {
                var result = GetFreeTime(day.AddDays(count), BarberId, totalDuration);
                var reservationDate = day.AddDays(count);
                if (result != null && result.Count != 0)
                {


                    foreach (var item in result)
                    {
                        var ObjResult = new AppResultVm() { From = item.From, To = item.To, Date = reservationDate };
                        MoreAppo.Add(ObjResult);
                    }
                }


                count++;



            }
            while (count <= 6);

            //var reservationDate = day.AddDays(count);


            return MoreAppo;
        }



        public List<TimeSlots> GetFreeTime(DateTime day, int BarberId, int totalDuration)
        {

            var DateName = day.DayOfWeek.ToString();
            var barber = _context.Barbers.Where(e => e.BarberId == BarberId).FirstOrDefault();
            var daybreaks = _context.WeekDays.Where(e => e.WeekDayId == barber.OffWeekDayId).FirstOrDefault().WeekDayTitle;
            DateTime dayStart = new DateTime(day.Year, day.Month, day.Day, 10, 0, 0);
            DateTime dayEnd = new DateTime(day.Year, day.Month, day.Day, 22, 0, 0);
            // Check if the selected date is the barber's day off
            if (day.DayOfWeek.ToString() == daybreaks)
            {
                // Barber is not available on this day, so no free slots
                return new List<TimeSlots>();
            }
            DateTime localNow = DateTime.Now.ToLocalTime();

            if (day.Date == DateTime.Now.Date && localNow > dayStart)
            {
                dayStart = localNow;
            }
            // Get all existing appointments for the selected date
            var existingAppointments = _context.Appointments
           .Where(a => a.AppointmentCreateDate.Value.Date == day.Date && a.BaberId == BarberId)
           .ToList();
            List<TimeSlots> timeSlots = new List<TimeSlots>();

            List<TimeSlots> Gaps = new List<TimeSlots>();

            if (existingAppointments != null && existingAppointments.Count != 0)
            {
                foreach (var item in existingAppointments)
                {
                    timeSlots.Add(new TimeSlots() { From = item.AppointmentStartDate.Value, To = item.AppointmentEndDate.Value });
                }
            }
            if (timeSlots != null && timeSlots.Count != 0)
            {
                var list = timeSlots.OrderBy(x => x.From).ToList();

                if (list[0].From > dayStart)
                {
                    var difference = list[0].From.Subtract(day);
                    if (difference.Hours >= totalDuration)
                        if (day <= dayEnd)
                            Gaps.Add(new TimeSlots() { From = day, To = list[0].From.AddHours(totalDuration) });
                }

                for (int i = 0; i < list.Count; i++)
                {



                    if (list[i].To == dayEnd.Date)
                    {
                        break;
                    }
                    if (i == list.Count - 1)
                    {
                        var difference = dayEnd.Subtract(list[i].To.AddMinutes(30));
                        if (difference.Hours >= totalDuration)
                            if (list[i].To.AddMinutes(30) <= dayEnd)
                            {
                                //Gaps.Add(new TimeSlots() { From = list[i].To.AddMinutes(30), To = dayEnd });

                                for (int x = 0; x < difference.Hours / totalDuration; x++)
                                {
                                    if (x == 0)
                                    {
                                        dayStart = list[i].To.AddMinutes(30);
                                    }

                                    Gaps.Add(new TimeSlots() { From = dayStart, To = dayStart.AddHours(totalDuration) });
                                    dayStart = dayStart.AddHours(totalDuration).AddMinutes(30);
                                    if (dayStart.AddHours(totalDuration) > dayEnd)
                                    {
                                        break;
                                    }
                                }
                            }
                    }
                    else
                    {
                        var difference = list[i + 1].From.Subtract(list[i].To.AddMinutes(30));
                        if (difference.Hours >= totalDuration)
                        {
                            if (list[i].To.AddMinutes(30) <= dayEnd)
                                Gaps.Add(new TimeSlots() { From = list[i].To.AddMinutes(30), To = list[i + 1].From });
                        }

                    }


                }
            }
            else
            {
                var daydifference = dayEnd - dayStart;
                var daydifferencehours = daydifference.Hours;
                var dayhours = daydifferencehours / totalDuration;
                for (int i = 0; i < dayhours; i++)
                {
                    if (i == 0)
                    {
                        if (day.Date == DateTime.Now.Date)
                        {
                            dayStart = DateTime.Now;
                        }

                    }
                    Gaps.Add(new TimeSlots() { From = dayStart, To = dayStart.AddHours(totalDuration) });
                    dayStart = dayStart.AddHours(totalDuration).AddMinutes(30);
                    if (dayStart.AddHours(totalDuration) > dayEnd)
                    {
                        break;
                    }
                }

            }
            return Gaps;


        }


        public async Task<IActionResult> OnPostMakeAppointment(AppointmentVM appointment)
        {
            try
            {

            
                List<TemporaryAppointmentService> temporaryAppointmentServicesList = new List<TemporaryAppointmentService>();
                
                TemporaryAppointmentService temporaryAppointmentService1 = new TemporaryAppointmentService()
                {
                    Amount = 4,
                    GenderId = 1,
                    ServiceId = 1,

                };
                temporaryAppointmentServicesList.Add(temporaryAppointmentService1);
                TemporaryAppointmentService temporaryAppointmentService2 = new TemporaryAppointmentService()
                {
                    Amount = 4,
                    GenderId = 1,
                    ServiceId = 2,

                };
                temporaryAppointmentServicesList.Add(temporaryAppointmentService2);
                appointment = new AppointmentVM()
                {
                    AppointmentCreateDate = DateTime.Now,
                    AppointmentFrom = DateTime.Now.AddHours(10),
                    AppointmentTo = DateTime.Now.AddHours(12),
                    BaberId = 1,
                    NumberOfKids = 2,
                    Remarks = "Remarks",
                    CustomerId = 1,
                    PaymentMethodId = 1,
                    TotalAmount = 8,
                    temporaryAppointmentServices= temporaryAppointmentServicesList

                };

                if (appointment == null)
                {
                    _toastNotification.AddErrorToastMessage("Enter Valid Data");
                    return Page();

                }
                var barber = _context.Barbers.Where(e => e.BarberId == appointment.BaberId).FirstOrDefault();
                if (barber == null)
                {
                    _toastNotification.AddErrorToastMessage("Enter Must Select Barber");
                    return Page();
                }
                var Customer = _context.Customers.Where(e => e.CustomerId == appointment.CustomerId).FirstOrDefault();

                if (Customer == null)
                {
                    _toastNotification.AddErrorToastMessage("Enter Must Select Customer");
                    return Page();
                }
                if (appointment.TotalAmount == 0)
                {
                    _toastNotification.AddErrorToastMessage("Enter Must Select Service");
                    return Page();
                }
                if (appointment.temporaryAppointmentServices == null)
                {
                    _toastNotification.AddErrorToastMessage("Enter Must Select Service");
                    return Page();
                }
                if (appointment.temporaryAppointmentServices.Count == 0)
                {
                    _toastNotification.AddErrorToastMessage("Enter Must Select Service");
                    return Page();
                }

                List<TemporaryAppointmentService> temporaryAppointmentServices = new List<TemporaryAppointmentService>();
                foreach (var item in appointment.temporaryAppointmentServices)
                {
                    var TemporayAppService = new TemporaryAppointmentService()
                    {
                        Amount = item.Amount,
                        GenderId = item.GenderId,
                        ServiceId = item.ServiceId,

                    };
                }

                var TemporaryAppointmentObj = new TemporaryAppointment()
                {
                    PaymentMethodId = appointment.PaymentMethodId,
                    TotalAmount = appointment.TotalAmount,
                    BaberId = appointment.BaberId,
                    CustomerId = appointment.CustomerId,
                    IsPaid = false,
                    Remarks = appointment.Remarks,
                    AppointmentStatusId = 1,
                    AppointmentCreateDate = DateTime.Now,
                    AppointmentStartDate = appointment.AppointmentFrom,
                    AppointmentEndDate = appointment.AppointmentTo,
                    TemporaryAppointmentServices = appointment.temporaryAppointmentServices,

                };

                _context.TemporaryAppointments.Add(TemporaryAppointmentObj);
                _context.SaveChanges();
                if (appointment.PaymentMethodId == 1)
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
                            InvoiceValue = TemporaryAppointmentObj.TotalAmount,
                            CallBackUrl = "http://SalonReservationSite.com/FattorahSuccess",
                            ErrorUrl = "http://SalonReservationSite.com/FattorahFailed",
                            UserDefinedField = TemporaryAppointmentObj.TemporaryAppointmentId,
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

                            _context.TemporaryAppointments.Remove(TemporaryAppointmentObj);
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
                            InvoiceValue = TemporaryAppointmentObj.TotalAmount,
                            CallBackUrl = "http://SalonReservationSite.com/FattorahSuccess",
                            ErrorUrl = "http://SalonReservationSite.com/FattorahFailed",
                            UserDefinedField = TemporaryAppointmentObj.TemporaryAppointmentId,
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

                            _context.TemporaryAppointments.Remove(TemporaryAppointmentObj);
                            _toastNotification.AddErrorToastMessage("Something Went Error Please Try Again");
                            RedirectToPage("SomethingwentError");

                        }
                    }


                }
               
            }


            catch (Exception ex)
            {
                return Page();

            }

            return Page();
        }


        //public IActionResult MakeAppointment([FromBody] AppointmentVM appointment)
        //{
        //	Random r = new Random();

        //	string RandomEmail = r.Next().ToString();
        //	if (appointment == null)
        //	{
        //		return Ok(new { Status = false, Message = "Object Equal Null,Please Send Objct Again.." });
        //	}
        //	if (appointment.EmployeeId == 0 || appointment.EmployeeId == null)
        //	{
        //		return Ok(new { Status = false, Message = "Employee Id Not Valid" });
        //	}
        //	if (appointment.CustomerId == 0 || appointment.CustomerId == null)
        //	{
        //		return Ok(new { Status = false, Message = "Customer Id Not Valid" });
        //	}
        //	if (appointment.Cost == 0)
        //	{
        //		return Ok(new { Status = false, Message = "Total Cost Must Be Large Than 0" });
        //	}
        //	if (appointment.NumberofHorses == 0)
        //	{
        //		return Ok(new { Status = false, Message = "Number of Hourses Must Be Large Than 0" });
        //	}
        //	if (appointment.AppointmentStatusId == 0)
        //	{
        //		return Ok(new { Status = false, Message = "Appointment Status  Id Not Valid" });
        //	}
        //	var ListOfDates = GetAppointment(appointment.NumberofHorses, appointment.EmployeeId.Value, appointment.CustomerId);
        //	var appointmentObj = new Appointments()
        //	{
        //		PaymentMethodId = 1,
        //		Cost = appointment.Cost,
        //		EmployeeId = appointment.EmployeeId,
        //		CustomerId = appointment.CustomerId,
        //		//Date = appointment.Date,
        //		ispaid = false,
        //		Lat = appointment.Lat,
        //		Lng = appointment.Lng,
        //		NumberofHorses = appointment.NumberofHorses,
        //		OrderSerialNumber = RandomEmail,
        //		//TimeTowill = appointment.TimeTowill,
        //		//TimeFrom = appointment.TimeFrom,
        //		Remarks = appointment.Remarks,
        //		CustomerAddress = appointment.CustomerAddress,
        //		AppointmentStatusId = appointment.AppointmentStatusId,
        //		AppointmentDetails = appointment.AppointmentDetails
        //	};
        //	try
        //	{
        //		_context.Appointments.Add(appointmentObj);
        //		_context.SaveChanges();
        //		List<AppoimentsDate> appoimentsDatesList = new List<AppoimentsDate>();
        //		foreach (var item in ListOfDates)
        //		{
        //			AppoimentsDate AppimentDate = new AppoimentsDate()
        //			{
        //				Date = item.Date,
        //				TimeFrom = item.From,
        //				TimeTowill = item.To,
        //				AppointmentsId = appointmentObj.AppointmentsId

        //			};
        //			appoimentsDatesList.Add(AppimentDate);
        //		}
        //		_context.AppoimentsDates.AddRange(appoimentsDatesList);
        //		_context.SaveChanges();
        //		Appointments UpdateAppoiments = UpdateAppo(ListOfDates, appointmentObj.AppointmentsId);
        //		return Ok(new { Status = true, Message = "Appointment Added Successfully", Appoiment = UpdateAppoiments, ListOfDates = ListOfDates });
        //	}
        //	catch (Exception ex)
        //	{
        //		return Ok(new { Status = false, Message = ex.Message });

        //	}

        //	//var GuidRandm = 
        //	// Random r = new Random();

        //	//string RandomSerialNumber = Guid.NewGuid().ToString();
        //	//int count = 0;
        //	//try
        //	//{
        //	//    List<Appointments> appointmentsList = new List<Appointments>();
        //	//foreach (AppointmentVM appointment in ListModel)
        //	//{
        //	//    if (appointment.CustomerId == 0)
        //	//{
        //	//    return Ok(new { Statut=false,Message = $"There is no Customer for item{count}" });

        //	//}
        //	//if (appointment.NumberofHorses == 0)
        //	//{
        //	//    return Ok(new { Statut = false, Message = $"Please Enter No. of Horses for item{count}" });

        //	//}
        //	//if(appointment.EmployeeId == 0)
        //	//{
        //	//    return Ok(new { Statut = false, Message = $"There is no Employee for item{count}" });
        //	//}
        //	//if (appointment.Cost == 0)
        //	//{
        //	//    return Ok(new { Statut = false, Message = $"Cost Mst be more than 0 for item{count}" });
        //	//}
        //	//if (appointment.ServiceId == 0)
        //	//{
        //	//    return Ok(new { Statut = false, Message = $"There is no Service for item{count}" });
        //	//}
        //	//if (appointment.AppointmentStatusId == 0)
        //	//{
        //	//    return Ok(new { Statut = false,Message = $"There is no AppointmentStatus for item{count}" });
        //	//}
        //	//var servicecost = _context.Services.Find(appointment.ServiceId).Cost;
        //	//if (servicecost==0)
        //	//{
        //	//    return Ok(new { Statut = false,Message = $"There is no service cost for item{count}" });

        //	//}


        //	//    var appointmentObj = new Appointments()
        //	//    {
        //	//        PaymentMethodId = 1,
        //	//        Cost = appointment.Cost,
        //	//        EmployeeId = appointment.EmployeeId,
        //	//        CustomerId = appointment.CustomerId,
        //	//        Date = appointment.Date,
        //	//        ispaid = false,
        //	//        Lat = appointment.Lat,
        //	//        Lng = appointment.Lng,
        //	//        NumberofHorses = appointment.NumberofHorses,
        //	//        TimeTowill = appointment.TimeTowill,
        //	//        TimeFrom = appointment.TimeFrom,
        //	//        Remarks = appointment.Remarks,
        //	//        CustomerAddress = appointment.CustomerAddress,
        //	//        AppointmentStatusId = appointment.AppointmentStatusId,
        //	//       // ServiceId = appointment.ServiceId,
        //	//        OrderSerialNumber= RandomSerialNumber


        //	//    };
        //	//   // appointmentObj.AppointmentAdditionalTypes = appointment.AppointmentAdditionalTypes;
        //	//    appointmentsList.Add(appointmentObj);
        //	//    count++;
        //	//}
        //	//    _context.Appointments.AddRange(appointmentsList);
        //	//    _context.SaveChanges();
        //	//    return Ok(new { status=true, AppointmentList = appointmentsList });
        //	//}
        //	//catch (Exception ex)
        //	//{
        //	//    return Ok(new { status = false,Message = ex.Message });
        //	//}
        //}
    }
}
