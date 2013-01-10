using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Herrd.Extensions.interfaces;

namespace Herrd.Extensions.Providers.TrackServices
{
	public class Vimeo : ITrackService
	{
		public string Type { get; set; }
		public string Title { get; set; }
		public string Term { get; set; }
		public string EmbedUrl { get; set; }
		public string Artwork { get; set; }

		public Vimeo(string term, string title)
		{
			Term = term;
			Title = title;
			Type = "vimeo";
			GetInfoFromTerm();
		}

		private void GetInfoFromTerm()
		{
			// IFRAME
			if (Term.Contains("iframe"))
			{
				string[] first = Term.Split(new[] {"src=\""}, StringSplitOptions.RemoveEmptyEntries);
				string[] second = first[1].Split('?');
				EmbedUrl = second[0] + "?portrait=0&amp;color=cccccc";
			}
			// PLAYER URL
			if (Term.Contains("player.vimeo.com"))
			{
				string[] first = Term.Split(new[] { "?" }, StringSplitOptions.RemoveEmptyEntries);
				EmbedUrl = first[0] + "?portrait=0&amp;color=cccccc";
			}
			// URL
			else if (Term.Contains("vimeo.com/"))
			{
				string[] first = Term.Split(new[] { ".com/" }, StringSplitOptions.RemoveEmptyEntries);
				EmbedUrl = "http://player.vimeo.com/video/" + first[1] + "?portrait=0&amp;color=cccccc";
			}
			// STRING containing "vimeo"
			else
			{
				Type = "memo";
			}
		}
	}
}
