using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Herrd.Extensions.interfaces;

namespace Herrd.Extensions.Providers.TrackServices
{
	public class Spotify : ITrackService
	{

		#region Implementation of ITrackService

		public string Type { get; set; }
		public string Title { get; set; }
		public string Term { get; set; }
		public string EmbedUrl { get; set; }
		public string Artwork { get; set; }

		#endregion

		public Spotify(string term, string title)
		{
			Term = term;
			Title = title;
			Type = "spotify";
			GetToWork();
		}

		private void GetToWork()
		{
			if (Term.Contains("iframe"))
			{
				string[] first = Term.Split(new string[] {"<iframe src=\""}, StringSplitOptions.RemoveEmptyEntries);
				string[] second = first[1].Split('"');
				EmbedUrl = second[0];
			}
			if (Term.Contains("http"))
			{
				
			}
		}
	}
}
