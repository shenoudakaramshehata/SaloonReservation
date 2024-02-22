namespace SaloonReservation.Models
{
    public class BarberImage
    {
        public int BarberImageId { get; set; }
        public string? pic { get; set; }
        public int BarberId { get; set; }
        public Barber? Barber { get; set; }
        public string? picDescription { get; set; }
    }
}
