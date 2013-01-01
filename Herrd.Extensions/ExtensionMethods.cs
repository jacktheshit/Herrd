using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Herrd.Extensions.Providers.Members;

namespace Herrd.Extensions
{
	static class ExtensionMethods
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

	}
}
