using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SaloonReservation.Models
{
    public class City
    {
        public City()
        {
            Area = new HashSet<Area>();
        }

        public int CityId { get; set; }
        public string CityTlAr { get; set; }
        public string CityTlEn { get; set; }
        public bool CityIsActive { get; set; }
        public int CityOrderIndex { get; set; }
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }
        public virtual ICollection<Area> Area { get; set; }
    }
}
