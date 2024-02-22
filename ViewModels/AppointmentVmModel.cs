namespace SaloonReservation.ViewModels
{
	public class AppointmentVmModel
	{
		public int AppointmentId { get; set; }
		public string AppointmentStatus { get; set; }

		public string AppointmentCreateDate { get; set; }
		public string Day{ get; set; }

		public string AppointmentStartDate { get; set; }

		public string AppointmentEndDate { get; set; }
        /// <summary>
        public DateTime AppointmentCreateDateNotFormat { get; set; }

        public DateTime AppointmentStartDateNotFormat { get; set; }

        public DateTime AppointmentEndDateNotFormat { get; set; }
        /// </summary>
        public string CustomerName { get; set; }
		public string CustomerPhone { get; set; }
		public string CustomerAddress { get; set; }
		public string CustomerCountry { get; set; }
		public string CustomerCity { get; set; }
		public string CustomerArea { get; set; }
		public string CustomerLat { get; set; }
		public string CustomerLng { get; set; }
		public string CustomerEmail { get; set; }
		public string Remarks { get; set; }

		public double TotalAmount { get; set; }

		public string BarberName { get; set; }
		public string SiteEmail { get; set; }
		public string SitePhone { get; set; }
		public string PaymentMethod { get; set; }
		public string PaymentId { get; set; }
		public string TwitterLink { get; set; }
		public string Instgramlink { get; set; }
		public double TotalDuration { get; set; }
		public string BarberPhone { get; set; }
		public int BarberId { get; set; }
		public string BarberEmail { get; set; }
		public string BarberImage { get; set; }
		public List<AppointmentServiceVM> Services { get; set; }
		
	}
}
