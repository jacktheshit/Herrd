using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Herrd.Extensions.Providers.TrackServices;
using Herrd.Extensions.interfaces;

namespace Herrd.Extensions
{
	public class ProcessTrack
	{

		private string Term { get; set; }
		private string Title { get; set; }

		private const string Spotify = "spotify";
		private const string Soundcloud = "soundcloud";
		private const string Vimeo = "vimeo";
		private const string Youtube = "youtu";

		public ProcessTrack(string term, string title)
		{
			Term = term;
			Title = title;
		}

		public ITrackService GetService()
		{
			if (Term.Contains(Spotify)) return new Spotify(Term, Title);
			if (Term.Contains(Soundcloud)) return new Spotify(Term, Title);
			if (Term.Contains(Vimeo)) return new Spotify(Term, Title);
			if (Term.Contains(Youtube)) return new Spotify(Term, Title);
			return null;
		}

	}
}
