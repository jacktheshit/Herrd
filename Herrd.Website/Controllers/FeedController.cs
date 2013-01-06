using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Herrd.DataLayer;

namespace Herrd.Website.Controllers
{
	public class FeedController : Controller
	{

		private readonly HerrdDBDataContext _db = new HerrdDBDataContext();

		public ActionResult Index()
		{
			return View(_db.Tracks.OrderByDescending(x => x.date));
		}

	}
}
