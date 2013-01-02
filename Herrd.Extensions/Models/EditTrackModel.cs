using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herrd.Extensions.Models
{
	public class EditTrackModel
	{
		[Required]
		public string Term { get; set; }

		[Required]
		public string Title { get; set; }
	}
}
