using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Herrd.DataLayer;

namespace Herrd.Website.Controllers
{
	public class HomeController : Controller
	{

		private HerrdEntities _db;

		public HomeController()
		{
			_db = new HerrdEntities();
		}

		public ActionResult Index()
		{
			ViewData.Model = _db.Tracks.ToList();
			return View("Index");
		}

	}
}
