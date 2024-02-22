using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace SaloonReservation.Models
{
	public class TemporaryAppointment
	{
		[Key]
		public int TemporaryAppointmentId { get; set; }

		public int AppointmentStatusId { get; set; }

		public DateTime AppointmentCreateDate { get; set; }

		public DateTime AppointmentStartDate { get; set; }

		public DateTime AppointmentEndDate { get; set; }

		public int? CustomerId { get; set; }
		public string Remarks { get; set; }

		public double TotalAmount { get; set; }

		public int BaberId { get; set; }
		public int PaymentMethodId { get; set; }
		public bool IsPaid { get; set; }
		public virtual ICollection<TemporaryAppointmentService>TemporaryAppointmentServices { get; set; }


	}
}
