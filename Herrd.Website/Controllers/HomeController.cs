using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Herrd.DataLayer;
using Herrd.Extensions.Models;

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
		public ActionResult AddTrack (AddTrackModel trackModel)
		{
			if (ModelState.IsValid)
			{
				var track = new Track
				{
					title = trackModel.Title,
					term = trackModel.Term,
					archive = false,
					playlist = false,
					date = DateTime.Now
				};

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
			else
			{
				return HttpNotFound();
			}
		}

		[HttpPost]
		public ActionResult EditTrack (Track track)
		{
			Track dbTrack = _dbUser.Tracks.FirstOrDefault(x => x.id == track.id);
			if (dbTrack == null) return HttpNotFound();

			dbTrack.term = track.term;
			dbTrack.title = track.title;
			try
			{
				_db.SubmitChanges();
			}
			catch(Exception e)
			{
				return HttpNotFound();
			}
			return RedirectToAction("Index");
		}

		[HttpPost]
		public ActionResult DeleteTrack(int id = 0)
		{
			Track dbTrack = _dbUser.Tracks.FirstOrDefault(x => x.id == id);
			if (dbTrack == null) return HttpNotFound();

			//delete
			_db.Tracks.DeleteOnSubmit(dbTrack);

			try
			{
				_db.SubmitChanges();
			}
			catch (Exception e)
			{
				return HttpNotFound();
			}

			return RedirectToAction("Index");
		}

	}
}