﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Herrd.Extensions.interfaces;

namespace Herrd.Extensions.Providers.TrackServices
{
	public class Soundcloud : ITrackService
	{
		public string Type { get; set; }
		public string Title { get; set; }
		public string Term { get; set; }
		public string EmbedUrl { get; set; }
		public string Artwork { get; set; }

		public Soundcloud (string term, string title)
		{
			Term = term;
			Title = title;
			Type = "soundcloud";
			GetInfoFromTerm();
		}

		private void GetInfoFromTerm()
		{

		}
	}
}
