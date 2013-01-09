using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herrd.Extensions
{
	public class ProcessTrack
	{

		public string Term { get; set; }
		public string Title { get; set; }

		public ProcessTrack(string term, string title)
		{
			Term = term;
			Title = title;
		}

	}
}
