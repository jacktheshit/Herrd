﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using LowercaseRoutesMVC4;

namespace Herrd.Website
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{

			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			//feed
			routes.MapRouteLowercase(
				name: "Feed",
				url: "feed/{action}",
				defaults: new { controller = "Feed", action = "Index" }
			);

			//archive
			routes.MapRouteLowercase(
				name: "Archive",
				url: "archive/{action}",
				defaults: new { controller = "Archive", action = "Index" }
			);

			//home
			routes.MapRouteLowercase(
				name: "Home",
				url: "{action}",
				defaults: new { controller = "Home", action = "Index" }
			);

			//default
			routes.MapRouteLowercase(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);

		}
	}
}