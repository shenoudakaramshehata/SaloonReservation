using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaloonReservation.Models
{
	public class Barber
	{
		public int BarberId { get; set; }
		public string? FullName { get; set; }
		public string? Email { get; set; }
		public string? Password { get; set; }
		public string? Description { get; set; }
		public string? Image { get; set; }
		[RegularExpression("^[0-9]+$", ErrorMessage = " Accept Number Only")]
		public string? Phone { get; set; }
        public int? OffWeekDayId { get; set; }

        public WeekDay OffWeekDay { get; set; }

        //[EmailAddress]
        //[Required, RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Not Valid")]
        //public string? Email { get; set; }
        //[NotMapped]
        //[DataType(DataType.Password)]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{6,}$", ErrorMessage = "Should have at least one lower case , one upper case,one number , one special character and minimum length 6 characters")]
        //public string? Password { get; set; }

        //[NotMapped]
        //[DataType(DataType.Password)]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{6,}$", ErrorMessage = "Should have at least one lower case , one upper case,one number , one special character and minimum length 6 characters")]
        //public string? OldPassword { get; set; }
        public bool IsActive { get; set; }
		public List <Appointment>appoitments { get; set; }
		public List<BarberImage>?BarberImages { get; set; }
	}
}
