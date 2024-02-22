using System.ComponentModel.DataAnnotations;

namespace SaloonReservation.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        [EmailAddress]
        [Required, RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Not Valid")]
        public string? Email { get; set; }
        [RegularExpression("^[0-9]+$", ErrorMessage = " Accept Number Only")]
        public string? Phone { get; set; }
        public string? FullName { get; set; }
        public int? CityId { get; set; }
        public City?  City { get; set; }
        public int? AreaId { get; set; }
        public Area? Area { get; set; }
        public string? FullAddress { get; set; }

        public int CountryId { get; set; }
        public Country Country { get; set; }
        public string? Lat { get; set; }
        public string? Lng { get; set; }
        public string? MapLocation { get; set; }



    }
}
