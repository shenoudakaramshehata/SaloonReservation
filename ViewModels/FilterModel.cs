
using System;

namespace SaloonReservation.ViewModels

{
    public class FilterModel
    {
        public DateTime? FromDate { set; get; }
        public DateTime? ToDate { set; get; }
        public DateTime? OnDay { set; get; }
        public int? BarberId { get; set; }
        public string? radioBtn { get; set; }
    }
}
