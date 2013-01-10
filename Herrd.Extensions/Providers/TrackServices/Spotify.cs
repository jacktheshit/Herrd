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
		public string Type { get; set; }
		public string Title { get; set; }
		public string Term { get; set; }
		public string EmbedUrl { get; set; }
		public string Artwork { get; set; }

		public Spotify(string term, string title)
		{
			Term = term;
			Title = title;
			Type = "spotify";
			GetInfoFromTerm();
		}

		private void GetInfoFromTerm()
		{
			// IFRAME
			if (Term.Contains("iframe"))
			{
				string[] first = Term.Split(new[] {"<iframe src=\""}, StringSplitOptions.RemoveEmptyEntries);
				string[] second = first[0].Split('"');
				EmbedUrl = second[0];
			}
			// HTTP LINK
			if (Term.Contains("http"))
			{
				if (Term.Contains("/album"))
				{
					string[] first = Term.Split(new[] {"http://open.spotify.com/album/"}, StringSplitOptions.RemoveEmptyEntries);
					EmbedUrl = "https://embed.spotify.com/?uri=spotify:album:" + first[0];
				}
				if (Term.Contains("/track"))
				{
					string[] first = Term.Split(new[] { "http://open.spotify.com/track/" }, StringSplitOptions.RemoveEmptyEntries);
					EmbedUrl = "https://embed.spotify.com/?uri=spotify:track:" + first[0];
				}
				if(Term.Contains("/playlist"))
				{
					
				}
			}
			// URI
			if (Term.Contains("spotify:"))
			{
				if (Term.Contains("album:"))
				{
					
				}
				if (Term.Contains(":track"))
				{
					
				}
				if (Term.Contains(":playlist"))
				{
					
				}
			}
		}
	}
}
