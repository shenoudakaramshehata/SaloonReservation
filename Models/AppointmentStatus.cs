using System.ComponentModel.DataAnnotations;

namespace SaloonReservation.Models
{
    public class AppointmentStatus
    {
        public int AppointmentStatusId { get; set; }
        [Required]
        public string? AppointmentStatusTitleEN { get; set; }
        [Required]
        public string? AppointmentStatusTitleAR { get; set; }
    }
}
