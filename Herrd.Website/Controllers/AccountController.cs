using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Herrd.Extensions.Providers.Members;
using WebMatrix.WebData;
using Herrd.Extensions.Models;

namespace Herrd.Website.Controllers
{
	public class AccountController : Controller
	{
		//
		// GET: /Account/

		public ActionResult Index()
		{
			return RedirectToAction("SignUp");
		}

		public ActionResult SignUp()
		{
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult SignUp(RegisterModel model)
		{
			if (ModelState.IsValid)
			{
				// Attempt to register the user
				try
				{
					MembershipCreateStatus createStatus;
					Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, true, null, out createStatus);
					
					if (createStatus == MembershipCreateStatus.Success)
					{
						FormsAuthentication.SetAuthCookie(model.Email, false /* createPersistentCookie */);
						return RedirectToAction("Index", "Home");
					}

					ModelState.AddModelError("", createStatus.ToString());
				}
				catch (MembershipCreateUserException e)
				{
					ModelState.AddModelError("", e.StatusCode.ToString());
				}
			}

			// If we got this far, something failed, redisplay form
			return View("SignUp");
		}

		public ActionResult Login ()
		{
			return RedirectToAction("SignUp");
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult Login(LoginModel model)
		{
			if (ModelState.IsValid)
			{
				if (Membership.ValidateUser(model.Email, model.Password))
				{
					FormsAuthentication.SetAuthCookie(model.Email, model.RememberMe);
					return RedirectToAction("Index", "Home");
				}
				else
				{
					ModelState.AddModelError("", "The email or password provided is incorrect.");
				}
			}

			//do this if shit fails
			ModelState.AddModelError("", "The email or password provided is incorrect.");
			return View("SignUp");
		}

		public ActionResult LogOut()
		{
			FormsAuthentication.SignOut();
			return RedirectToAction("Index", "Home");
		}

		[Authorize]
		public ActionResult EditProfile()
		{
			return View(new EditProfileModel());
		}

		[Authorize]
		[HttpPost]
		public ActionResult EditProfile(EditProfileModel model)
		{
			return HttpNotFound();
		}

	}
}
