using System;

namespace SaloonReservation.Models
{
    public class AppResultVm
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public DateTime Date { get; set; }
        public int NumberOfKids { get; set; }
    }
}
