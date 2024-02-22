namespace SaloonReservation.Models
{
    public class ContactUs
    {
        public int ContactUsID { get; set; }
        public string? Name { get; set; }
        public string? Message { get; set; }
        public string? Email { get; set; }
        public DateTime? SendingDate { get; set; }
    }
}
