using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herrd.Extensions.Models
{
	public class LoginModel
	{
		[Required]
		[DataType(DataType.EmailAddress)]
		[Display(Name = "Email Address")]
		public string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[Display(Name = "Keep me logged in")]
		public bool RememberMe { get; set; }
	}
}
