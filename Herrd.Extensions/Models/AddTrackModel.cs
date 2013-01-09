using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Herrd.Extensions.Models
{
	public class AddTrackModel
	{

		public string Title { get; set; }

		[AllowHtml]
		[Required]
		public string Term { get; set; }

	}
}
