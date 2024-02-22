using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SaloonReservation.Models
{
    public class Area
    {
        public int AreaId { get; set; }
        public int CityId { get; set; }
        public string AreaTlAr { get; set; }
        public string AreaTlEn { get; set; }
        public bool AreaIsActive { get; set; }
        public int AreaOrderIndex { get; set; }
        public virtual City City { get; set; }
    }
}
