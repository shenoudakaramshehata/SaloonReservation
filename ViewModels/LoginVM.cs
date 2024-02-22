using System.ComponentModel.DataAnnotations;

namespace SaloonReservation.ViewModels
{
	public class LoginVM
	{
		[Required(ErrorMessage = "Enter your email")]
		[EmailAddress]
		public string Email { get; set; }

		/// <summary>
		///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
		///     directly from your code. This API may change or be removed in future releases.
		/// </summary>
		[Required(ErrorMessage = "Enter your password")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		/// <summary>
		///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
		///     directly from your code. This API may change or be removed in future releases.
		/// </summary>
		[Display(Name = "Remember me?")]
		public bool RememberMe { get; set; }
	}
}
