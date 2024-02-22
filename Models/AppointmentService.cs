using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SaloonReservation.Models
{
    public class AppointmentService
    {
        [Key]
        public int AppointmentServiceId { get; set; }

        [Required]
        public int AppointmentId { get; set; }

        [Required]
        public int ServiceId { get; set; }
		
		public int NumberOfKids { get; set; }

		public int? GenderId { get; set; }

        public double? Amount { get; set; }

        [ForeignKey("AppointmentId")]
        public Appointment? Appointment { get; set; }

        [ForeignKey("ServiceId")]
        public Service? Service { get; set; }

        [ForeignKey("GenderId")]
        public Gender? Gender { get; set; }
    }
}
