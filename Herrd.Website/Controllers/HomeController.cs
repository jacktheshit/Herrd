using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Herrd.DataLayer;

namespace Herrd.Website.Controllers
{
	public class HomeController : Controller
	{

		private readonly HerrdDBDataContext _db = new HerrdDBDataContext();

		public ActionResult Index()
		{
			var tracks = _db.Tracks.Include(t => t.User);
			return View(tracks.Where(x => !x.archive).OrderByDescending(x => x.date).ToList());
		}

		public bool AddToArchive(int id = 0)
		{
			if (id == 0) return false;
			Track track = _db.Tracks.FirstOrDefault(x => x.id == id);

			if (track == null) return false;
			track.archive = true;
			_db.SubmitChanges();
			return true;
		}

		public bool RestoreFromArchive (int id = 0)
		{
			if (id == 0) return false;
			Track track = _db.Tracks.FirstOrDefault(x => x.id == id);

			if (track == null) return false;
			track.archive = false;
			_db.SubmitChanges();
			return true;
		}

		[HttpPost]
		public ActionResult AddTrack (Track track)
		{
			//get user
			User user = _db.Users.FirstOrDefault(x => x.id == 3);

			//update track object to have defaults
			track.archive = false;
			track.playlist = false;
			track.date = DateTime.Now;

			//add track to user
			user.Tracks.Add(track);

			//save
			try
			{
				_db.SubmitChanges();
				return PartialView("~/Views/Partials/TrackItemNormal.cshtml", track);
			}
			catch (Exception e)
			{
				return HttpNotFound();
			}
		}

		public ActionResult DeleteRecord(int id = 0)
		{
			return HttpNotFound();
		}

	}
}