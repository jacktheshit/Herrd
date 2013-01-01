using System.Web.Security;
using System.Configuration.Provider;
using System.Collections.Specialized;
using System;
using System.Linq;
using System.Data;
using System.Data.Odbc;
using System.Configuration;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Web.Configuration;
using Herrd.DataLayer;

/*

This provider works with the following schema for the table of user data.

CREATE TABLE Users
(
  PKID Guid NOT NULL PRIMARY KEY,
  Username Text (255) NOT NULL,
  ApplicationName Text (255) NOT NULL,
  Email Text (128) NOT NULL,
  Comment Text (255),
  Password Text (128) NOT NULL,
  PasswordQuestion Text (255),
  PasswordAnswer Text (255),
  IsApproved YesNo, 
  LastActivityDate DateTime,
  LastLoginDate DateTime,
  LastPasswordChangedDate DateTime,
  CreationDate DateTime, 
  IsOnLine YesNo,
  IsLockedOut YesNo,
  LastLockedOutDate DateTime,
  FailedPasswordAttemptCount Integer,
  FailedPasswordAttemptWindowStart DateTime,
  FailedPasswordAnswerAttemptCount Integer,
  FailedPasswordAnswerAttemptWindowStart DateTime
)

*/


namespace Herrd.Extensions.Providers.Members
{

	public sealed class HerrdMembershipProvider : MembershipProvider
	{

		private HerrdDBDataContext _dbDataContext;

		//
		// Global connection string, generated password length, generic exception message, event log info.
		//
		private const int NewPasswordLength = 8;
		private const string EventSource = "HerrdMembershipProvider";
		private const string EventLog = "Application";
		private const string ExceptionMessage = "An exception occurred. Please check the Event Log.";
		private string _connectionString;

		//
		// Used when determining encryption key values.
		//
		private MachineKeySection _machineKey;

		//
		// If false, exceptions are thrown to the caller. If true,
		// exceptions are written to the event log.
		//
		private bool _pWriteExceptionsToEventLog;

		public bool WriteExceptionsToEventLog
		{
			get { return _pWriteExceptionsToEventLog; }
			set { _pWriteExceptionsToEventLog = value; }
		}


		//
		// System.Configuration.Provider.ProviderBase.Initialize Method
		//

		public override void Initialize(string name, NameValueCollection config)
		{

			//init dbContext
			_dbDataContext = new HerrdDBDataContext();

			//
			// Initialize values from web.config.
			//

			if (config == null)
				throw new ArgumentNullException("config");

			if (name.Length == 0)
				name = "HerrdMembershipProvider";

			if (String.IsNullOrEmpty(config["description"]))
			{
				config.Remove("description");
				config.Add("description", "Herrd Membership provider");
			}

			// Initialize the abstract base class.
			base.Initialize(name, config);

			_pApplicationName = GetConfigValue(config["applicationName"],
											System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
			_pMaxInvalidPasswordAttempts = Convert.ToInt32(GetConfigValue(config["maxInvalidPasswordAttempts"], "5"));
			_pPasswordAttemptWindow = Convert.ToInt32(GetConfigValue(config["passwordAttemptWindow"], "10"));
			_pMinRequiredNonAlphanumericCharacters = Convert.ToInt32(GetConfigValue(config["minRequiredNonAlphanumericCharacters"], "1"));
			_pMinRequiredPasswordLength = Convert.ToInt32(GetConfigValue(config["minRequiredPasswordLength"], "7"));
			_pPasswordStrengthRegularExpression = Convert.ToString(GetConfigValue(config["passwordStrengthRegularExpression"], ""));
			_pEnablePasswordReset = Convert.ToBoolean(GetConfigValue(config["enablePasswordReset"], "true"));
			_pEnablePasswordRetrieval = Convert.ToBoolean(GetConfigValue(config["enablePasswordRetrieval"], "true"));
			_pRequiresQuestionAndAnswer = Convert.ToBoolean(GetConfigValue(config["requiresQuestionAndAnswer"], "false"));
			_pRequiresUniqueEmail = Convert.ToBoolean(GetConfigValue(config["requiresUniqueEmail"], "true"));
			_pWriteExceptionsToEventLog = Convert.ToBoolean(GetConfigValue(config["writeExceptionsToEventLog"], "true"));

			string tempFormat = config["passwordFormat"] ?? "Hashed";

			switch (tempFormat)
			{
				case "Hashed":
					_pPasswordFormat = MembershipPasswordFormat.Hashed;
					break;
				case "Encrypted":
					_pPasswordFormat = MembershipPasswordFormat.Encrypted;
					break;
				case "Clear":
					_pPasswordFormat = MembershipPasswordFormat.Clear;
					break;
				default:
					throw new ProviderException("Password format not supported.");
			}

			//
			// Initialize OdbcConnection.
			//

			ConnectionStringSettings connectionStringSettings =
			  ConfigurationManager.ConnectionStrings[config["connectionStringName"]];

			if (connectionStringSettings == null || connectionStringSettings.ConnectionString.Trim() == "")
			{
				throw new ProviderException("Connection string cannot be blank.");
			}

			_connectionString = connectionStringSettings.ConnectionString;


			// Get encryption and decryption key information from the configuration.
			Configuration cfg =
			  WebConfigurationManager.OpenWebConfiguration(System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
			_machineKey = (MachineKeySection)cfg.GetSection("system.web/machineKey");

			if (_machineKey.ValidationKey.Contains("AutoGenerate"))
				if (PasswordFormat != MembershipPasswordFormat.Clear)
					throw new ProviderException("Hashed or Encrypted passwords " +
												"are not supported with auto-generated keys.");
		}


		//
		// A helper function to retrieve config values from the configuration file.
		//

		private string GetConfigValue(string configValue, string defaultValue)
		{
			if (String.IsNullOrEmpty(configValue))
				return defaultValue;

			return configValue;
		}


		//
		// System.Web.Security.MembershipProvider properties.
		//


		private string _pApplicationName;
		private bool _pEnablePasswordReset;
		private bool _pEnablePasswordRetrieval;
		private bool _pRequiresQuestionAndAnswer;
		private bool _pRequiresUniqueEmail;
		private int _pMaxInvalidPasswordAttempts;
		private int _pPasswordAttemptWindow;
		private MembershipPasswordFormat _pPasswordFormat;

		public override string ApplicationName
		{
			get { return _pApplicationName; }
			set { _pApplicationName = value; }
		}

		public override bool EnablePasswordReset
		{
			get { return _pEnablePasswordReset; }
		}

		public override bool EnablePasswordRetrieval
		{
			get { return _pEnablePasswordRetrieval; }
		}

		public override bool RequiresQuestionAndAnswer
		{
			get { return _pRequiresQuestionAndAnswer; }
		}

		public override bool RequiresUniqueEmail
		{
			get { return _pRequiresUniqueEmail; }
		}

		public override int MaxInvalidPasswordAttempts
		{
			get { return _pMaxInvalidPasswordAttempts; }
		}

		public override int PasswordAttemptWindow
		{
			get { return _pPasswordAttemptWindow; }
		}

		public override MembershipPasswordFormat PasswordFormat
		{
			get { return _pPasswordFormat; }
		}

		private int _pMinRequiredNonAlphanumericCharacters;

		public override int MinRequiredNonAlphanumericCharacters
		{
			get { return _pMinRequiredNonAlphanumericCharacters; }
		}

		private int _pMinRequiredPasswordLength;

		public override int MinRequiredPasswordLength
		{
			get { return _pMinRequiredPasswordLength; }
		}

		private string _pPasswordStrengthRegularExpression;

		public override string PasswordStrengthRegularExpression
		{
			get { return _pPasswordStrengthRegularExpression; }
		}

		//
		// System.Web.Security.MembershipProvider methods.
		//

		//
		// MembershipProvider.ChangePassword
		//

		public override bool ChangePassword(string username, string oldPwd, string newPwd)
		{
			if (!ValidateUser(username, oldPwd))
				return false;

			ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, newPwd, true);

			OnValidatingPassword(args);

			if (args.Cancel)
				if (args.FailureInformation != null)
					throw args.FailureInformation;
				else
					throw new MembershipPasswordException("Change password canceled due to new password validation failure.");

			//get user and update password
			User user = _dbDataContext.Users.FirstOrDefault(u => u.user_name == username);
			if (user == null) return false;
			user.password = EncodePassword(newPwd);

			try
			{
				_dbDataContext.SubmitChanges();
			}
			catch(Exception e)
			{
				return false;
			}

			return true;

		}



		//
		// MembershipProvider.ChangePasswordQuestionAndAnswer
		//

		public override bool ChangePasswordQuestionAndAnswer(string username,
					  string password,
					  string newPwdQuestion,
					  string newPwdAnswer)
		{
			if (!ValidateUser(username, password))
				return false;

			OdbcConnection conn = new OdbcConnection(_connectionString);
			OdbcCommand cmd = new OdbcCommand("UPDATE Users " +
					" SET PasswordQuestion = ?, PasswordAnswer = ?" +
					" WHERE Username = ? AND ApplicationName = ?", conn);

			cmd.Parameters.Add("@Question", OdbcType.VarChar, 255).Value = newPwdQuestion;
			cmd.Parameters.Add("@Answer", OdbcType.VarChar, 255).Value = EncodePassword(newPwdAnswer);
			cmd.Parameters.Add("@Username", OdbcType.VarChar, 255).Value = username;
			cmd.Parameters.Add("@ApplicationName", OdbcType.VarChar, 255).Value = _pApplicationName;


			int rowsAffected = 0;

			try
			{
				conn.Open();

				rowsAffected = cmd.ExecuteNonQuery();
			}
			catch (OdbcException e)
			{
				if (WriteExceptionsToEventLog)
				{
					WriteToEventLog(e, "ChangePasswordQuestionAndAnswer");

					throw new ProviderException(ExceptionMessage);
				}
				else
				{
					throw e;
				}
			}
			finally
			{
				conn.Close();
			}

			if (rowsAffected > 0)
			{
				return true;
			}

			return false;
		}


		//
		// MembershipProvider.CreateUser
		//

		public override MembershipUser CreateUser(string username,
				 string password,
				 string email,
				 string passwordQuestion,
				 string passwordAnswer,
				 bool isApproved,
				 object providerUserKey,
				 out MembershipCreateStatus status)
		{
			ValidatePasswordEventArgs args =
			  new ValidatePasswordEventArgs(username, password, true);

			OnValidatingPassword(args);

			if (args.Cancel)
			{
				status = MembershipCreateStatus.InvalidPassword;
				return null;
			}

			if (RequiresUniqueEmail && GetUserNameByEmail(email) != "")
			{
				status = MembershipCreateStatus.DuplicateEmail;
				return null;
			}

			MembershipUser u = GetUser(username, false);

			if (u == null)
			{
				DateTime createDate = DateTime.Now;

				User newUser = new User
				{
					user_name = username,
					password = EncodePassword(password),
					email = email,
					creationDate = createDate,
					lastActivityDate = createDate,
					lastLoginDate = createDate,
					first_name = "",
					last_name = "",
					isApproved = isApproved,
					userRole = HeerdRoleProvider.RoleType.User.GetRoleType()
				};

				try
				{
					_dbDataContext.Users.InsertOnSubmit(newUser);
					_dbDataContext.SubmitChanges();
					status = MembershipCreateStatus.Success;
				}
				catch (Exception e)
				{
					status = MembershipCreateStatus.ProviderError;
					throw new Exception(e.Message);
				}

				return GetUser(username, false);
			}
			else
			{
				status = MembershipCreateStatus.DuplicateUserName;
			}


			return null;
		}

		//
		// MembershipProvider.DeleteUser
		//

		public override bool DeleteUser(string username, bool deleteAllRelatedData)
		{

			User userToDelete = _dbDataContext.Users.FirstOrDefault(u => u.user_name == username);
			if (userToDelete == null) return false;

			//delete tracks
			var usersTracks = _dbDataContext.Tracks.Where(x => x.user_id == userToDelete.id);
			foreach (var usersTrack in usersTracks)
			{
				_dbDataContext.Tracks.DeleteOnSubmit(usersTrack);
			}

			//delete user
			_dbDataContext.Users.DeleteOnSubmit(userToDelete);

			try
			{
				_dbDataContext.SubmitChanges();
				return true;
			}
			catch (Exception)
			{

				return false;
			}
		}



		//
		// MembershipProvider.GetAllUsers
		//

		public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
		{
			OdbcConnection conn = new OdbcConnection(_connectionString);
			OdbcCommand cmd = new OdbcCommand("SELECT Count(*) FROM Users " +
											  "WHERE ApplicationName = ?", conn);
			cmd.Parameters.Add("@ApplicationName", OdbcType.VarChar, 255).Value = ApplicationName;

			MembershipUserCollection users = new MembershipUserCollection();

			OdbcDataReader reader = null;
			totalRecords = 0;

			try
			{
				conn.Open();
				totalRecords = (int)cmd.ExecuteScalar();

				if (totalRecords <= 0) { return users; }

				cmd.CommandText = "SELECT PKID, Username, Email, PasswordQuestion," +
						 " Comment, IsApproved, IsLockedOut, CreationDate, LastLoginDate," +
						 " LastActivityDate, LastPasswordChangedDate, LastLockedOutDate " +
						 " FROM Users " +
						 " WHERE ApplicationName = ? " +
						 " ORDER BY Username Asc";

				reader = cmd.ExecuteReader();

				int counter = 0;
				int startIndex = pageSize * pageIndex;
				int endIndex = startIndex + pageSize - 1;

				while (reader.Read())
				{
					if (counter >= startIndex)
					{
						//MembershipUser u = GetUserFromReader(reader);
						//users.Add(u);
					}

					if (counter >= endIndex) { cmd.Cancel(); }

					counter++;
				}
			}
			catch (OdbcException e)
			{
				if (WriteExceptionsToEventLog)
				{
					WriteToEventLog(e, "GetAllUsers ");

					throw new ProviderException(ExceptionMessage);
				}
				else
				{
					throw e;
				}
			}
			finally
			{
				if (reader != null) { reader.Close(); }
				conn.Close();
			}

			return users;
		}


		//
		// MembershipProvider.GetNumberOfUsersOnline
		//

		public override int GetNumberOfUsersOnline()
		{

			TimeSpan onlineSpan = new TimeSpan(0, System.Web.Security.Membership.UserIsOnlineTimeWindow, 0);
			DateTime compareTime = DateTime.Now.Subtract(onlineSpan);

			OdbcConnection conn = new OdbcConnection(_connectionString);
			OdbcCommand cmd = new OdbcCommand("SELECT Count(*) FROM Users " +
					" WHERE LastActivityDate > ? AND ApplicationName = ?", conn);

			cmd.Parameters.Add("@CompareDate", OdbcType.DateTime).Value = compareTime;
			cmd.Parameters.Add("@ApplicationName", OdbcType.VarChar, 255).Value = _pApplicationName;

			int numOnline = 0;

			try
			{
				conn.Open();

				numOnline = (int)cmd.ExecuteScalar();
			}
			catch (OdbcException e)
			{
				if (WriteExceptionsToEventLog)
				{
					WriteToEventLog(e, "GetNumberOfUsersOnline");

					throw new ProviderException(ExceptionMessage);
				}
				else
				{
					throw e;
				}
			}
			finally
			{
				conn.Close();
			}

			return numOnline;
		}


		//
		// MembershipProvider.GetPassword
		//

		public override string GetPassword(string username, string answer)
		{
			if (!EnablePasswordRetrieval)
			{
				throw new ProviderException("Password Retrieval Not Enabled.");
			}

			if (PasswordFormat == MembershipPasswordFormat.Hashed)
			{
				throw new ProviderException("Cannot retrieve Hashed passwords.");
			}

			User user = _dbDataContext.Users.FirstOrDefault(x => x.user_name == username);

			string password = "";

			if (user == null)
			{
				throw new MembershipPasswordException("The supplied user name is not found.");
			}
			else
			{
				password = user.password;
			}

			if (PasswordFormat == MembershipPasswordFormat.Encrypted)
			{
				password = UnEncodePassword(password);
			}

			return password;
		}

		//
		// MembershipProvider.ResetPassword
		//
		//TODO: Reset Password
		public override string ResetPassword(string username, string answer)
		{
			if (!EnablePasswordReset)
			{
				throw new NotSupportedException("Password reset is not enabled.");
			}

			if (answer == null && RequiresQuestionAndAnswer)
			{
				throw new ProviderException("Password answer required for password reset.");
			}

			string newPassword = Membership.GeneratePassword(NewPasswordLength, MinRequiredNonAlphanumericCharacters);


			ValidatePasswordEventArgs args =
			  new ValidatePasswordEventArgs(username, newPassword, true);

			OnValidatingPassword(args);

			if (args.Cancel)
				if (args.FailureInformation != null)
					throw args.FailureInformation;
				else
					throw new MembershipPasswordException("Reset password canceled due to password validation failure.");


			OdbcConnection conn = new OdbcConnection(_connectionString);
			OdbcCommand cmd = new OdbcCommand("SELECT PasswordAnswer, IsLockedOut FROM Users " +
				  " WHERE Username = ? AND ApplicationName = ?", conn);

			cmd.Parameters.Add("@Username", OdbcType.VarChar, 255).Value = username;
			cmd.Parameters.Add("@ApplicationName", OdbcType.VarChar, 255).Value = _pApplicationName;

			int rowsAffected = 0;
			string passwordAnswer = "";
			OdbcDataReader reader = null;

			try
			{
				conn.Open();

				reader = cmd.ExecuteReader(CommandBehavior.SingleRow);

				if (reader.HasRows)
				{
					reader.Read();

					if (reader.GetBoolean(1))
						throw new MembershipPasswordException("The supplied user is locked out.");

					passwordAnswer = reader.GetString(0);
				}
				else
				{
					throw new MembershipPasswordException("The supplied user name is not found.");
				}

				if (RequiresQuestionAndAnswer && !CheckPassword(answer, passwordAnswer))
				{
					

					throw new MembershipPasswordException("Incorrect password answer.");
				}

				OdbcCommand updateCmd = new OdbcCommand("UPDATE Users " +
					" SET Password = ?, LastPasswordChangedDate = ?" +
					" WHERE Username = ? AND ApplicationName = ? AND IsLockedOut = False", conn);

				updateCmd.Parameters.Add("@Password", OdbcType.VarChar, 255).Value = EncodePassword(newPassword);
				updateCmd.Parameters.Add("@LastPasswordChangedDate", OdbcType.DateTime).Value = DateTime.Now;
				updateCmd.Parameters.Add("@Username", OdbcType.VarChar, 255).Value = username;
				updateCmd.Parameters.Add("@ApplicationName", OdbcType.VarChar, 255).Value = _pApplicationName;

				rowsAffected = updateCmd.ExecuteNonQuery();
			}
			catch (OdbcException e)
			{
				if (WriteExceptionsToEventLog)
				{
					WriteToEventLog(e, "ResetPassword");

					throw new ProviderException(ExceptionMessage);
				}
				else
				{
					throw e;
				}
			}
			finally
			{
				if (reader != null) { reader.Close(); }
				conn.Close();
			}

			if (rowsAffected > 0)
			{
				return newPassword;
			}
			else
			{
				throw new MembershipPasswordException("User not found, or user is locked out. Password not Reset.");
			}
		}

		//
		// GetUserFromReader
		//    A helper function that takes the current row from the OdbcDataReader
		// and hydrates a MembershiUser from the values. Called by the 
		// MembershipUser.GetUser implementation.
		//

		private MembershipUser GetUserFromHerrdUser(User user)
		{
			object providerUserKey = user.id;
			string username = user.user_name;
			string email = user.email;

			//not implemented stuff
			const string passwordQuestion = "";
			string comment = string.Empty;

			bool isApproved = user.isApproved;
			const bool isLockedOut = false;
			DateTime creationDate = user.creationDate ?? DateTime.Now;

			DateTime lastLoginDate =  user.lastLoginDate ?? DateTime.Now;

			DateTime lastActivityDate = user.lastActivityDate ?? DateTime.Now;
			DateTime lastPasswordChangedDate = DateTime.Now;

			DateTime lastLockedOutDate = DateTime.Now;

			var u = new MembershipUser(Name,
										username,
										providerUserKey,
										email,
										passwordQuestion,
										comment,
										isApproved,
										isLockedOut,
										creationDate,
										lastLoginDate,
										lastActivityDate,
										lastPasswordChangedDate,
										lastLockedOutDate);

			return u;
		}

		//
		// CheckPassword
		//   Compares password values based on the MembershipPasswordFormat.
		//

		private bool CheckPassword(string password, string dbpassword)
		{
			string pass1 = password;
			string pass2 = dbpassword;

			switch (PasswordFormat)
			{
				case MembershipPasswordFormat.Encrypted:
					pass2 = UnEncodePassword(dbpassword);
					break;
				case MembershipPasswordFormat.Hashed:
					pass1 = EncodePassword(password);
					break;
				default:
					break;
			}

			if (pass1 == pass2)
			{
				return true;
			}

			return false;
		}


		//
		// EncodePassword
		//   Encrypts, Hashes, or leaves the password clear based on the PasswordFormat.
		//

		private string EncodePassword(string password)
		{
			string encodedPassword = password;

			switch (PasswordFormat)
			{
				case MembershipPasswordFormat.Clear:
					break;
				case MembershipPasswordFormat.Encrypted:
					encodedPassword =
					  Convert.ToBase64String(EncryptPassword(Encoding.Unicode.GetBytes(password)));
					break;
				case MembershipPasswordFormat.Hashed:
					HMACSHA1 hash = new HMACSHA1();
					hash.Key = HexToByte(_machineKey.ValidationKey);
					encodedPassword =
					  Convert.ToBase64String(hash.ComputeHash(Encoding.Unicode.GetBytes(password)));
					break;
				default:
					throw new ProviderException("Unsupported password format.");
			}

			return encodedPassword;
		}


		//
		// UnEncodePassword
		//   Decrypts or leaves the password clear based on the PasswordFormat.
		//

		private string UnEncodePassword(string encodedPassword)
		{
			string password = encodedPassword;

			switch (PasswordFormat)
			{
				case MembershipPasswordFormat.Clear:
					break;
				case MembershipPasswordFormat.Encrypted:
					password =
					  Encoding.Unicode.GetString(DecryptPassword(Convert.FromBase64String(password)));
					break;
				case MembershipPasswordFormat.Hashed:
					throw new ProviderException("Cannot unencode a hashed password.");
				default:
					throw new ProviderException("Unsupported password format.");
			}

			return password;
		}

		//
		// HexToByte
		//   Converts a hexadecimal string to a byte array. Used to convert encryption
		// key values from the configuration.
		//

		private byte[] HexToByte(string hexString)
		{
			byte[] returnBytes = new byte[hexString.Length / 2];
			for (int i = 0; i < returnBytes.Length; i++)
				returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
			return returnBytes;
		}

		//
		// WriteToEventLog
		//   A helper function that writes exception detail to the event log. Exceptions
		// are written to the event log as a security measure to avoid private database
		// details from being returned to the browser. If a method does not return a status
		// or boolean indicating the action succeeded or failed, a generic exception is also 
		// thrown by the caller.
		//

		private void WriteToEventLog(Exception e, string action)
		{
			EventLog log = new EventLog();
			log.Source = EventSource;
			log.Log = EventLog;

			string message = "An exception occurred communicating with the data source.\n\n";
			message += "Action: " + action + "\n\n";
			message += "Exception: " + e.ToString();

			log.WriteEntry(message);
		}


		//Not implemented

		public override bool ValidateUser(string username, string password)
		{
			//this will actually validate again the email not the username as we login with email
			User user = _dbDataContext.Users.FirstOrDefault(x => x.email == username);

			bool isValid = false;

			if (user == null) return false;

			if (CheckPassword(password, user.password))
			{
				if (user.isApproved)
				{
					isValid = true;
					user.lastLoginDate = DateTime.Now;
					try
					{
						_dbDataContext.SubmitChanges();
					}
					catch (Exception e)
					{
						//TODO: sort out logging
					}
				}
			}

			return isValid;

		}

		public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
		{
			throw new NotImplementedException();
		}

		public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
		{
			throw new NotImplementedException();
		}

		public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
		{
			throw new NotImplementedException();
		}

		public override MembershipUser GetUser(string username, bool userIsOnline)
		{
			//using email instead of username :|
			User user = _dbDataContext.Users.FirstOrDefault(u => u.email == username);
			return user == null ? null : GetUserFromHerrdUser(user);
		}

		public override string GetUserNameByEmail(string email)
		{
			User user = _dbDataContext.Users.FirstOrDefault(u => u.email == email);
			return user == null ? String.Empty : user.user_name;
		}

		public override bool UnlockUser(string userName)
		{
			throw new NotImplementedException();
		}

		public override void UpdateUser(MembershipUser user)
		{
			throw new NotImplementedException();
		}

	}
}