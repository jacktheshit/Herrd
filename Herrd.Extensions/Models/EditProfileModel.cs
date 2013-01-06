using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herrd.Extensions.Models
{
	public class EditProfileModel
	{

		[Display(Name = "First name")]
		public string Firstname { get; set; }

		[Display(Name = "Last name")]
		public string Surname { get; set; }

		[Required]
		[Display(Name = "Username")]
		public string Username { get; set; }

		[Required]
		[DataType(DataType.EmailAddress)]
		[Display(Name = "Email address")]
		public string Email { get; set; }

		[Display(Name = "Website")]
		public string Website { get; set; }

		[Display(Name = "City")]
		public string City { get; set; }

		[Display(Name = "Country")]
		public string Country { get; set; }

		[Required]
		[Display(Name = "Profile privacy")]
		public bool IsPrivate { get; set; }

		[Required]
		public bool IsPublic { get; set; }

	}
}
