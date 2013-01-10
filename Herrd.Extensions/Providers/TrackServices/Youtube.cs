using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Herrd.Extensions.interfaces;

namespace Herrd.Extensions.Providers.TrackServices
{
	public class Youtube : ITrackService
	{
		public string Type { get; set; }
		public string Title { get; set; }
		public string Term { get; set; }
		public string EmbedUrl { get; set; }
		public string Artwork { get; set; }

		public Youtube(string term, string title)
		{
			Term = term;
			Title = title;
			Type = "youtube";
			GetInfoFromTerm();
		}

		private void GetInfoFromTerm()
		{
			// EMBED CODE
			if (Term.Contains("iframe"))
			{
				string[] first = Term.Split(new[] { "src=\"" }, StringSplitOptions.RemoveEmptyEntries);
				string[] second = first[1].Split('?');
				EmbedUrl = second[0] + "?rel=0";
			}
			// SHORT URL
			if (Term.Contains("youtu.be"))
			{
				string[] first = Term.Split(new[] { ".be/" }, StringSplitOptions.RemoveEmptyEntries);
				EmbedUrl = "http://www.youtube.com/embed/" + first[1] + "?rel=0";
			}
			// FULL URL
			else if (Term.Contains("youtube.com"))
			{
				string[] first = Term.Split(new[] { "v=" }, StringSplitOptions.RemoveEmptyEntries);
				string[] second = first[1].Split('&');
				EmbedUrl = "http://www.youtube.com/embed/" + second[0] + "?rel=0";
			}
			// STRING containing "youtube"
			else
			{
				Type = "memo";
			}
		}
	}
}
