using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Herrd.Extensions.Providers.Members;

namespace Herrd.Extensions
{
	public static class ExtensionMethods
	{

		public static string GetRoleType (this HeerdRoleProvider.RoleType enumValue)
		{
			switch (enumValue)
			{
				case HeerdRoleProvider.RoleType.Admin :
					return "HerrdAdmin";
				case HeerdRoleProvider.RoleType.User:
					return "HerrdUser";
			}
			return "HerrdUser";
		}

		public static IEnumerable<SelectListItem> ToSelectListItems(this List<string> list, string selectedId)
		{
			return list.Select(x =>
				new SelectListItem
				{
					Selected = (x == selectedId),
					Text = x,
					Value = x
				}
			);
		}

	}
}
