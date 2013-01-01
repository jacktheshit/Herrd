using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Herrd.DataLayer;

namespace Herrd.Website.Controllers
{
	public class HomeController : Controller
	{

		private readonly HerrdDBDataContext _db = new HerrdDBDataContext();
		private readonly MembershipUser _currentUser;
		private readonly User _dbUser;

		public HomeController()
		{
			_currentUser = Membership.GetUser();
			if (_currentUser != null) _dbUser = _db.Users.FirstOrDefault(x => x.email == _currentUser.Email);
		}

		public ActionResult Index()
		{
			//no user then send to alt homepage
			if (_currentUser == null) return RedirectToAction("NormalHomepage");

			//otherwise show filtered list of tracks
			if (_dbUser != null)
			{
				var tracks = _dbUser.Tracks;
				return View(tracks.Where(x => !x.archive).OrderByDescending(x => x.date).ToList());
			}
			return RedirectToAction("NormalHomepage");
		}

		public ActionResult NormalHomepage ()
		{
			return View();
		}

		public bool AddToArchive(int id = 0)
		{
			if (id == 0) return false;
			Track track = _dbUser.Tracks.FirstOrDefault(x => x.id == id);

			if (track == null) return false;
			track.archive = true;
			_db.SubmitChanges();
			return true;
		}

		public bool RestoreFromArchive (int id = 0)
		{
			if (id == 0) return false;
			Track track = _dbUser.Tracks.FirstOrDefault(x => x.id == id);

			if (track == null) return false;
			track.archive = false;
			_db.SubmitChanges();
			return true;
		}

		[HttpPost]
		public ActionResult AddTrack (Track track)
		{
			//update track object to have defaults
			track.archive = false;
			track.playlist = false;
			track.date = DateTime.Now;

			//add track to user
			if (_dbUser == null) return HttpNotFound();
			_dbUser.Tracks.Add(track);

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