using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Herrd.DataLayer;
using Herrd.Extensions;
using Herrd.Extensions.Providers.Members;
using WebMatrix.WebData;
using Herrd.Extensions.Models;
using Herrd.Extensions;

namespace Herrd.Website.Controllers
{
	public class AccountController : Controller
	{
		private readonly HerrdDBDataContext _db = new HerrdDBDataContext();
		private readonly MembershipUser _currentUser;
		private readonly User _dbUser;

		public AccountController()
		{
			_currentUser = Membership.GetUser();
			if (_currentUser != null) _dbUser = _db.Users.FirstOrDefault(x => x.email == _currentUser.Email);
			if (_dbUser != null) _countryList = Helpers.CountryList.ToSelectListItems(_dbUser.country);
		}

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

		private readonly IEnumerable<SelectListItem> _countryList;
		
		[Authorize]
		public ActionResult EditProfile()
		{
			//set some defaults for the radio buttons
			var model = new EditProfileModel
			{
				Username = _dbUser.user_name,
				Firstname = _dbUser.first_name,
				Surname = _dbUser.last_name,
				Email = _dbUser.email,
				City = _dbUser.city,
				Website = _dbUser.websiteUrl,
				Countries = _countryList,
				IsPublic = !_dbUser.isPrivate
			};
			return View(model);
		}

		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult EditProfile(EditProfileModel model)
		{
			if (ModelState.IsValid)
			{
				_dbUser.user_name = model.Username;
				_dbUser.first_name = model.Firstname;
				_dbUser.last_name = model.Surname;
				_dbUser.email = model.Email;
				_dbUser.city = model.City;
				_dbUser.websiteUrl = model.Website;
				_dbUser.country = model.Country;
				_dbUser.isPrivate = !model.IsPublic;

				_dbUser.lastActivityDate = DateTime.Now;

				try
				{
					_db.SubmitChanges();
				}
				catch(Exception e)
				{
					
				}

				model.Countries = _countryList;
				return View(model);
			}

			return View();
		}

	}
}
