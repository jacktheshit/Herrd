using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herrd.Extensions.interfaces
{
	public interface ITrackService
	{

		string Type { get; set; }
		string Title { get; set; }
		string Term { get; set; }
		string EmbedUrl { get; set; }
		string Artwork { get; set; }

	}
}
