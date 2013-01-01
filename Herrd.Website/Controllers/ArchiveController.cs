using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Herrd.DataLayer;

namespace Herrd.Website.Controllers
{
	public class ArchiveController : Controller
	{

		private readonly HerrdDBDataContext _db = new HerrdDBDataContext();
		private readonly MembershipUser _currentUser;
		private readonly User _dbUser;

		public ArchiveController()
		{
			_currentUser = Membership.GetUser();
			if (_currentUser != null) _dbUser = _db.Users.FirstOrDefault(x => x.email == _currentUser.Email);
		}

		public ActionResult Index()
		{
			//redirect to homepage if not logged in
			if (_dbUser == null) return RedirectToAction("NormalHomepage", "Home");

			var tracks = _dbUser.Tracks;
			return View(tracks.Where(x => x.archive).OrderByDescending(x => x.date).ToList());
		}

	}
}
