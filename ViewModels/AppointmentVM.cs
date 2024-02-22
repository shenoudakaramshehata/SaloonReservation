using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaloonReservation.Models
{
    public class AppointmentVM
    {
		public int NumberOfKids { get; set; }

		public DateTime AppointmentCreateDate { get; set; }

		public DateTime AppointmentFrom { get; set; }

		public DateTime AppointmentTo { get; set; }
		public int CustomerId { get; set; }
		public string Remarks { get; set; }
		public float TotalAmount { get; set; }
		public int BaberId { get; set; }
		public int PaymentMethodId { get; set; }
		public List<TemporaryAppointmentService> temporaryAppointmentServices { get; set; }

    }
}
