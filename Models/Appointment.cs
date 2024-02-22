using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SaloonReservation.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }

        [Required]
        public int AppointmentStatusId { get; set; }

        public DateTime? AppointmentCreateDate { get; set; }

        public DateTime? AppointmentStartDate { get; set; }

        public DateTime? AppointmentEndDate { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [MaxLength]
        public string? Remarks { get; set; }
        public string? FattorahPaymentId { get; set; }

        public double? TotalAmount { get; set; }

        public int? BaberId { get; set; }

        [ForeignKey("AppointmentStatusId")]
        public AppointmentStatus? AppointmentStatus { get; set; }

        [ForeignKey("CustomerId")]
        public Customer? Customer { get; set; }

        [ForeignKey("BaberId")]
        public Barber? Barber { get; set; }

        public List<AppointmentService> Services { get; set; }
    }
}
