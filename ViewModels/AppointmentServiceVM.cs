namespace SaloonReservation.ViewModels
{
	public class AppointmentServiceVM
	{
		public string ServiceTitle { get; set; }

		public string Gender { get; set; }

		public double Amount { get; set; }
		public double OneServiceAmount { get; set; }
		public int ServiceDuration { get; set; }
		public int NumberOfKids { get; set; }
		public string ServiceImage { get; set; }
	}
}
