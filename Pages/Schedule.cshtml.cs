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

namespace SaloonReservation.Pages
{
    public class ScheduleModel : PageModel
    {
		List<AppResultVm> MoreAppo = new List<AppResultVm>();
		public List<DisplaydaySlots> dayTimeSLot = new List<DisplaydaySlots>();
		public List<TemporaryAppointmentService> temporaryAppointmentServices = new List<TemporaryAppointmentService>();
        public List<Barber> Barbers = new List<Barber>();
        public List<Gender> Genders = new List<Gender>();
		
		public int AppointmentId { get; set; }
		public TemporaryAppointment TemporaryAppoiment { get; set; }
        private SalonContext _context;
        private readonly IToastNotification _toastNotification;
        public IRequestCultureFeature locale;
        public string BrowserCulture;
		private int count = 0;
		public ScheduleModel(SalonContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
           
        }
        public IActionResult OnGet(int AppoimentId)
        {
			AppointmentId = AppoimentId;
            TemporaryAppoiment = _context.TemporaryAppointments.Include(e=>e.TemporaryAppointmentServices).Where(e => e.TemporaryAppointmentId == AppoimentId).FirstOrDefault();
            if (TemporaryAppoiment==null)
            {
                return Redirect("/AppoimentServices");

            }
			List<int> service = new List<int>();
			foreach (var item in TemporaryAppoiment.TemporaryAppointmentServices)
			{
				var serviceObj=_context.Services.Where(e => e.ServiceId == item.ServiceId).FirstOrDefault();
				var totalDuration = item.NumberOfKids * serviceObj.Duration;
				service.Add(totalDuration);

            }


            //GetAppointment(service, DateTime.Now, TemporaryAppoiment.BaberId);
            //var DateTimeOfKwait= TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time"));
            TimeZoneInfo timeZoneInfo;
            DateTime dateTime;
            //Set the time zone information to US Mountain Standard Time 
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
            //Get date and time in US Mountain Standard Time 
            dateTime = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var MoreAppoments = GetAppointment(service, dateTime, TemporaryAppoiment.BaberId);
			var MoreAppomentsList = MoreAppoments.GroupBy(e => e.Date.Date).ToList();

			dayTimeSLot = new List<DisplaydaySlots>();
			foreach (var appointmentsForDate in MoreAppomentsList)
			{
				var dayTimeSLotobj = new DisplaydaySlots();
				List<SlotsList> SlotsList = new List<SlotsList>();
				foreach (var item in appointmentsForDate)
				{
                    string fromTimeString = item.From.TimeOfDay.ToString();
                    string ToTimeString = item.To.TimeOfDay.ToString();
					string formattedTime = "";
                    string formattedTimeTo = "";
                    string[] timeComponents = fromTimeString.Split(':');
                    string[] timeComponentsTo = ToTimeString.Split(':');
                    if (timeComponents.Length >= 2)
					{
						// Extract hours and minutes components
						string hours = timeComponents[0];
						string minutes = timeComponents[1];
                        if (int.TryParse(hours, out int parsedHours) && int.TryParse(minutes, out int parsedMinutes))
                        {
                            // Format the hours and minutes in 12-hour format with AM/PM
                             formattedTime = $"{parsedHours:D2}:{parsedMinutes:D2} {(parsedHours >= 12 ? "PM" : "AM")}";
                            
                            
                            Console.WriteLine(formattedTime); // Output: "10:53 AM" (example)
                        }
                    }


                    if (timeComponentsTo.Length >= 2)
                    {
                        // Extract hours and minutes components
                        string hours = timeComponentsTo[0];
                        string minutes = timeComponentsTo[1];
                        if (int.TryParse(hours, out int parsedHours) && int.TryParse(minutes, out int parsedMinutes))
                        {
                            // Format the hours and minutes in 12-hour format with AM/PM
                            formattedTimeTo = $"{parsedHours:D2}:{parsedMinutes:D2} {(parsedHours >= 12 ? "PM" : "AM")}";
                            

							Console.WriteLine(formattedTimeTo); // Output: "10:53 AM" (example)
                        }
                    }
               
                    var slotlistobj = new SlotsList()
                    {
                        From = formattedTime,
                        DateFrom = item.From,
                        To = formattedTimeTo,
                        DateTo = item.To,
                    };
                   
                    SlotsList.Add(slotlistobj);
				}
				dayTimeSLotobj.Date = appointmentsForDate.Key;
				dayTimeSLotobj.slotsLists = SlotsList;

				dayTimeSLot.Add(dayTimeSLotobj);
			}
			return Page();

		}

		public IActionResult OnPost (int AppoimentId)
		{
			try
			{
				var day = Request.Form["dayofWeeks"];
				DateTime dateTime;
                if (DateTime.TryParseExact(day, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
                {
                    // The parsing was successful, and `dateTime` now contains the DateTime value.
                    Console.WriteLine(dateTime);
                }
                var timeSlot = Request.Form["timeSlot"].ToString();
			   string[] timeSlots = timeSlot.Split(',');
				DateTime fromDate = Convert.ToDateTime(timeSlots[0]);
				DateTime ToDate = Convert.ToDateTime(timeSlots[1]);
                var appointment= _context.TemporaryAppointments.Where(e=>e.TemporaryAppointmentId== AppoimentId).FirstOrDefault();
				if(appointment != null) {
					appointment.AppointmentCreateDate = dateTime;
					appointment.AppointmentStartDate = fromDate;
					appointment.AppointmentEndDate = ToDate;
                    _context.Attach(appointment).State = EntityState.Modified;

                     _context.SaveChanges();
                }
                return Redirect($"/FastReservation?AppoimentId={AppoimentId}");
            }
			catch(Exception e)
			{
				return RedirectToPage("SomethingwentError");
			}
			
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
			while (count <= 7);

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
            TimeZoneInfo timeZoneInfo;
            DateTime dateTime;
            //Set the time zone information to US Mountain Standard Time 
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
            //Get date and time in US Mountain Standard Time 
            dateTime = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            DateTime localNow = dateTime.ToLocalTime();

			if (day.Date == dateTime.Date && localNow > dayStart)
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
                        
                        //Set the time zone information to US Mountain Standard Time 
                        timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
                        //Get date and time in US Mountain Standard Time 
                        dateTime = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
                        if (day.Date == dateTime.Date)
						{
							dayStart = dateTime;
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
	}
}
