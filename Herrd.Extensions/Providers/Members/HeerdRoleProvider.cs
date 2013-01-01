using System.Web.Security;
using System.Configuration.Provider;
using System.Collections.Specialized;
using System;
using System.Linq;
using System.Data;
using System.Data.Odbc;
using System.Configuration;
using System.Diagnostics;
using System.Web;
using System.Globalization;

using Herrd.Extensions;
using Herrd.DataLayer;

namespace Herrd.Extensions.Providers.Members
{
	public sealed class HeerdRoleProvider : RoleProvider
	{

		public enum RoleType
		{
			Admin = 0,
			User = 1
		}

		private HerrdDBDataContext _dbContent;

		//
		// System.Configuration.Provider.ProviderBase.Initialize Method
		//

		public override void Initialize(string name, NameValueCollection config)
		{

			//
			// Initialize values from web.config.
			//

			if (config == null)
				throw new ArgumentNullException("config");

			if (name.Length == 0)
				name = "HerrdRoleProvider";

			if (String.IsNullOrEmpty(config["description"]))
			{
				config.Remove("description");
				config.Add("description", "Sample ODBC Role provider");
			}

			// Initialize the abstract base class.
			base.Initialize(name, config);

			//
			// Initialize OdbcConnection.
			//
			_dbContent = new HerrdDBDataContext();
		}

		public override void AddUsersToRoles(string[] usernames, string[] roleNames)
		{
			throw new NotImplementedException();
		}

		public override string ApplicationName
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public override void CreateRole(string roleName)
		{
			throw new NotImplementedException();
		}

		public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
		{
			throw new NotImplementedException();
		}

		public override string[] FindUsersInRole(string roleName, string usernameToMatch)
		{

			throw new NotImplementedException();
		}

		public override string[] GetAllRoles()
		{
			throw new NotImplementedException();
		}

		public override string[] GetRolesForUser(string username)
		{
			User user = _dbContent.Users.FirstOrDefault(u => u.user_name == username);
			return user == null ? null : new string[]{user.userRole};
		}

		public override string[] GetUsersInRole(string roleName)
		{
			return _dbContent.Users.Where(u => u.userRole == roleName).Select(u => u.user_name).ToArray();
		}

		public override bool IsUserInRole(string username, string roleName)
		{
			User user = _dbContent.Users.FirstOrDefault(x => x.user_name == username);
			if (user == null) throw new ProviderException("No user found");
			return user.userRole == roleName;
		}

		public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
		{
			throw new NotImplementedException();
		}

		public override bool RoleExists(string roleName)
		{
			throw new NotImplementedException();
		}

	}
}
