using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace SaloonReservation.Models
{
	public class TemporaryAppointmentService
	{
		[Key]
		public int TemporaryAppointmentServiceId { get; set; }

		[Required]
		public int TemporaryAppointmentId { get; set; }

		[Required]
		public int ServiceId { get; set; }

		public int GenderId { get; set; }

		public double Amount { get; set; }
		public int NumberOfKids { get; set; }


	}
}
