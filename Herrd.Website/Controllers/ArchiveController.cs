using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Herrd.DataLayer;

namespace Herrd.Website.Controllers
{
	public class ArchiveController : Controller
	{

		private readonly HerrdDBDataContext _db = new HerrdDBDataContext();

		public ActionResult Index()
		{
			var tracks = _db.Tracks.Include(t => t.User);
			return View(tracks.Where(x => x.archive).OrderByDescending(x => x.date).ToList());
		}

	}
}
