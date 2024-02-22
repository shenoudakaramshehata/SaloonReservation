using System.ComponentModel.DataAnnotations;

namespace SaloonReservation.Models
{
    public class WeekDay
    {
        [Key]
        public int WeekDayId { get; set; }

        public int? WeekDayIndex { get; set; }

        [MaxLength(50)]
        public string? WeekDayTitle { get; set; }
    }
}
