namespace SaloonReservation.Models
{
    public class Service
    {
        public int ServiceId { get; set; }
        public string? serviceTlAr { get; set; }
        public string? serviceTlEn { get; set; }
        public double OneKidPrice { get; set; }
        public double MoreKidsPrice { get; set; }
        public int Duration { get; set; }
        public int GenderId { get; set; }
        public Gender? Gender { get; set; }
    }
}
