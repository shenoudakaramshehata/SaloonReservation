namespace SaloonReservation.Models
{
    public class Country
    {
        public Country()
        {
            City = new HashSet<City>();

        }

        public int CountryId { get; set; }
        public string CountryTlAr { get; set; }
        public string CountryTlEn { get; set; }
       
        public bool CountryIsActive { get; set; }
        public int? CountryOrderIndex { get; set; }

        public virtual ICollection<City> City { get; set; }
    }
}
