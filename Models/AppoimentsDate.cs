using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SaloonReservation.Models
{
    public class AppoimentsDate
    {
        [Key]
        public int AppoimentsDateId { get; set; }
        public DateTime Date { get; set; }

        public DateTime TimeFrom { get; set; }

        public DateTime TimeTowill { get; set; }
    
        public int AppointmentsId { get; set; }
       

    }
}
