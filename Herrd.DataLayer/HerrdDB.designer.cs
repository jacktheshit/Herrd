﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Herrd.DataLayer
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="devHerrd")]
	public partial class HerrdDBDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertTrack(Track instance);
    partial void UpdateTrack(Track instance);
    partial void DeleteTrack(Track instance);
    partial void InsertUser(User instance);
    partial void UpdateUser(User instance);
    partial void DeleteUser(User instance);
    #endregion
		
		public HerrdDBDataContext() : 
				base(global::Herrd.DataLayer.Properties.Settings.Default.devHerrdConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public HerrdDBDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public HerrdDBDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public HerrdDBDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public HerrdDBDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<Track> Tracks
		{
			get
			{
				return this.GetTable<Track>();
			}
		}
		
		public System.Data.Linq.Table<User> Users
		{
			get
			{
				return this.GetTable<User>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Tracks")]
	public partial class Track : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _id;
		
		private int _user_id;
		
		private string _title;
		
		private string _term;
		
		private bool _archive;
		
		private string _type;
		
		private System.Nullable<bool> _playlist;
		
		private string _embed_url;
		
		private string _artwork;
		
		private System.Nullable<System.DateTime> _date;
		
		private EntityRef<User> _User;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnidChanging(int value);
    partial void OnidChanged();
    partial void Onuser_idChanging(int value);
    partial void Onuser_idChanged();
    partial void OntitleChanging(string value);
    partial void OntitleChanged();
    partial void OntermChanging(string value);
    partial void OntermChanged();
    partial void OnarchiveChanging(bool value);
    partial void OnarchiveChanged();
    partial void OntypeChanging(string value);
    partial void OntypeChanged();
    partial void OnplaylistChanging(System.Nullable<bool> value);
    partial void OnplaylistChanged();
    partial void Onembed_urlChanging(string value);
    partial void Onembed_urlChanged();
    partial void OnartworkChanging(string value);
    partial void OnartworkChanged();
    partial void OndateChanging(System.Nullable<System.DateTime> value);
    partial void OndateChanged();
    #endregion
		
		public Track()
		{
			this._User = default(EntityRef<User>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int id
		{
			get
			{
				return this._id;
			}
			set
			{
				if ((this._id != value))
				{
					this.OnidChanging(value);
					this.SendPropertyChanging();
					this._id = value;
					this.SendPropertyChanged("id");
					this.OnidChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_user_id", DbType="Int NOT NULL")]
		public int user_id
		{
			get
			{
				return this._user_id;
			}
			set
			{
				if ((this._user_id != value))
				{
					if (this._User.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.Onuser_idChanging(value);
					this.SendPropertyChanging();
					this._user_id = value;
					this.SendPropertyChanged("user_id");
					this.Onuser_idChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_title", DbType="NVarChar(150)")]
		public string title
		{
			get
			{
				return this._title;
			}
			set
			{
				if ((this._title != value))
				{
					this.OntitleChanging(value);
					this.SendPropertyChanging();
					this._title = value;
					this.SendPropertyChanged("title");
					this.OntitleChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_term", DbType="NVarChar(150)")]
		public string term
		{
			get
			{
				return this._term;
			}
			set
			{
				if ((this._term != value))
				{
					this.OntermChanging(value);
					this.SendPropertyChanging();
					this._term = value;
					this.SendPropertyChanged("term");
					this.OntermChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_archive", DbType="Bit NOT NULL")]
		public bool archive
		{
			get
			{
				return this._archive;
			}
			set
			{
				if ((this._archive != value))
				{
					this.OnarchiveChanging(value);
					this.SendPropertyChanging();
					this._archive = value;
					this.SendPropertyChanged("archive");
					this.OnarchiveChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_type", DbType="NVarChar(50)")]
		public string type
		{
			get
			{
				return this._type;
			}
			set
			{
				if ((this._type != value))
				{
					this.OntypeChanging(value);
					this.SendPropertyChanging();
					this._type = value;
					this.SendPropertyChanged("type");
					this.OntypeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_playlist", DbType="Bit")]
		public System.Nullable<bool> playlist
		{
			get
			{
				return this._playlist;
			}
			set
			{
				if ((this._playlist != value))
				{
					this.OnplaylistChanging(value);
					this.SendPropertyChanging();
					this._playlist = value;
					this.SendPropertyChanged("playlist");
					this.OnplaylistChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_embed_url", DbType="NVarChar(150)")]
		public string embed_url
		{
			get
			{
				return this._embed_url;
			}
			set
			{
				if ((this._embed_url != value))
				{
					this.Onembed_urlChanging(value);
					this.SendPropertyChanging();
					this._embed_url = value;
					this.SendPropertyChanged("embed_url");
					this.Onembed_urlChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_artwork", DbType="NVarChar(150)")]
		public string artwork
		{
			get
			{
				return this._artwork;
			}
			set
			{
				if ((this._artwork != value))
				{
					this.OnartworkChanging(value);
					this.SendPropertyChanging();
					this._artwork = value;
					this.SendPropertyChanged("artwork");
					this.OnartworkChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_date", DbType="DateTime")]
		public System.Nullable<System.DateTime> date
		{
			get
			{
				return this._date;
			}
			set
			{
				if ((this._date != value))
				{
					this.OndateChanging(value);
					this.SendPropertyChanging();
					this._date = value;
					this.SendPropertyChanged("date");
					this.OndateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="User_Track", Storage="_User", ThisKey="user_id", OtherKey="id", IsForeignKey=true)]
		public User User
		{
			get
			{
				return this._User.Entity;
			}
			set
			{
				User previousValue = this._User.Entity;
				if (((previousValue != value) 
							|| (this._User.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._User.Entity = null;
						previousValue.Tracks.Remove(this);
					}
					this._User.Entity = value;
					if ((value != null))
					{
						value.Tracks.Add(this);
						this._user_id = value.id;
					}
					else
					{
						this._user_id = default(int);
					}
					this.SendPropertyChanged("User");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Users")]
	public partial class User : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _id;
		
		private string _first_name;
		
		private string _last_name;
		
		private string _email;
		
		private string _password;
		
		private bool _isApproved;
		
		private string _user_name;
		
		private System.Nullable<System.DateTime> _lastActivityDate;
		
		private System.Nullable<System.DateTime> _lastLoginDate;
		
		private System.Nullable<System.DateTime> _creationDate;
		
		private System.Nullable<bool> _isOnline;
		
		private string _userRole;
		
		private string _websiteUrl;
		
		private string _city;
		
		private string _country;
		
		private bool _isPrivate;
		
		private string _avatar;
		
		private EntitySet<Track> _Tracks;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnidChanging(int value);
    partial void OnidChanged();
    partial void Onfirst_nameChanging(string value);
    partial void Onfirst_nameChanged();
    partial void Onlast_nameChanging(string value);
    partial void Onlast_nameChanged();
    partial void OnemailChanging(string value);
    partial void OnemailChanged();
    partial void OnpasswordChanging(string value);
    partial void OnpasswordChanged();
    partial void OnisApprovedChanging(bool value);
    partial void OnisApprovedChanged();
    partial void Onuser_nameChanging(string value);
    partial void Onuser_nameChanged();
    partial void OnlastActivityDateChanging(System.Nullable<System.DateTime> value);
    partial void OnlastActivityDateChanged();
    partial void OnlastLoginDateChanging(System.Nullable<System.DateTime> value);
    partial void OnlastLoginDateChanged();
    partial void OncreationDateChanging(System.Nullable<System.DateTime> value);
    partial void OncreationDateChanged();
    partial void OnisOnlineChanging(System.Nullable<bool> value);
    partial void OnisOnlineChanged();
    partial void OnuserRoleChanging(string value);
    partial void OnuserRoleChanged();
    partial void OnwebsiteUrlChanging(string value);
    partial void OnwebsiteUrlChanged();
    partial void OncityChanging(string value);
    partial void OncityChanged();
    partial void OncountryChanging(string value);
    partial void OncountryChanged();
    partial void OnisPrivateChanging(bool value);
    partial void OnisPrivateChanged();
    partial void OnavatarChanging(string value);
    partial void OnavatarChanged();
    #endregion
		
		public User()
		{
			this._Tracks = new EntitySet<Track>(new Action<Track>(this.attach_Tracks), new Action<Track>(this.detach_Tracks));
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int id
		{
			get
			{
				return this._id;
			}
			set
			{
				if ((this._id != value))
				{
					this.OnidChanging(value);
					this.SendPropertyChanging();
					this._id = value;
					this.SendPropertyChanged("id");
					this.OnidChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_first_name", DbType="NVarChar(50)")]
		public string first_name
		{
			get
			{
				return this._first_name;
			}
			set
			{
				if ((this._first_name != value))
				{
					this.Onfirst_nameChanging(value);
					this.SendPropertyChanging();
					this._first_name = value;
					this.SendPropertyChanged("first_name");
					this.Onfirst_nameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_last_name", DbType="NVarChar(50)")]
		public string last_name
		{
			get
			{
				return this._last_name;
			}
			set
			{
				if ((this._last_name != value))
				{
					this.Onlast_nameChanging(value);
					this.SendPropertyChanging();
					this._last_name = value;
					this.SendPropertyChanged("last_name");
					this.Onlast_nameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_email", DbType="NVarChar(50)")]
		public string email
		{
			get
			{
				return this._email;
			}
			set
			{
				if ((this._email != value))
				{
					this.OnemailChanging(value);
					this.SendPropertyChanging();
					this._email = value;
					this.SendPropertyChanged("email");
					this.OnemailChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_password", DbType="NVarChar(50)")]
		public string password
		{
			get
			{
				return this._password;
			}
			set
			{
				if ((this._password != value))
				{
					this.OnpasswordChanging(value);
					this.SendPropertyChanging();
					this._password = value;
					this.SendPropertyChanged("password");
					this.OnpasswordChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_isApproved", DbType="Bit NOT NULL")]
		public bool isApproved
		{
			get
			{
				return this._isApproved;
			}
			set
			{
				if ((this._isApproved != value))
				{
					this.OnisApprovedChanging(value);
					this.SendPropertyChanging();
					this._isApproved = value;
					this.SendPropertyChanged("isApproved");
					this.OnisApprovedChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_user_name", DbType="NVarChar(150)")]
		public string user_name
		{
			get
			{
				return this._user_name;
			}
			set
			{
				if ((this._user_name != value))
				{
					this.Onuser_nameChanging(value);
					this.SendPropertyChanging();
					this._user_name = value;
					this.SendPropertyChanged("user_name");
					this.Onuser_nameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_lastActivityDate", DbType="DateTime")]
		public System.Nullable<System.DateTime> lastActivityDate
		{
			get
			{
				return this._lastActivityDate;
			}
			set
			{
				if ((this._lastActivityDate != value))
				{
					this.OnlastActivityDateChanging(value);
					this.SendPropertyChanging();
					this._lastActivityDate = value;
					this.SendPropertyChanged("lastActivityDate");
					this.OnlastActivityDateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_lastLoginDate", DbType="DateTime")]
		public System.Nullable<System.DateTime> lastLoginDate
		{
			get
			{
				return this._lastLoginDate;
			}
			set
			{
				if ((this._lastLoginDate != value))
				{
					this.OnlastLoginDateChanging(value);
					this.SendPropertyChanging();
					this._lastLoginDate = value;
					this.SendPropertyChanged("lastLoginDate");
					this.OnlastLoginDateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_creationDate", DbType="DateTime")]
		public System.Nullable<System.DateTime> creationDate
		{
			get
			{
				return this._creationDate;
			}
			set
			{
				if ((this._creationDate != value))
				{
					this.OncreationDateChanging(value);
					this.SendPropertyChanging();
					this._creationDate = value;
					this.SendPropertyChanged("creationDate");
					this.OncreationDateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_isOnline", DbType="Bit")]
		public System.Nullable<bool> isOnline
		{
			get
			{
				return this._isOnline;
			}
			set
			{
				if ((this._isOnline != value))
				{
					this.OnisOnlineChanging(value);
					this.SendPropertyChanging();
					this._isOnline = value;
					this.SendPropertyChanged("isOnline");
					this.OnisOnlineChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_userRole", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string userRole
		{
			get
			{
				return this._userRole;
			}
			set
			{
				if ((this._userRole != value))
				{
					this.OnuserRoleChanging(value);
					this.SendPropertyChanging();
					this._userRole = value;
					this.SendPropertyChanged("userRole");
					this.OnuserRoleChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_websiteUrl", DbType="NVarChar(150)")]
		public string websiteUrl
		{
			get
			{
				return this._websiteUrl;
			}
			set
			{
				if ((this._websiteUrl != value))
				{
					this.OnwebsiteUrlChanging(value);
					this.SendPropertyChanging();
					this._websiteUrl = value;
					this.SendPropertyChanged("websiteUrl");
					this.OnwebsiteUrlChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_city", DbType="NVarChar(50)")]
		public string city
		{
			get
			{
				return this._city;
			}
			set
			{
				if ((this._city != value))
				{
					this.OncityChanging(value);
					this.SendPropertyChanging();
					this._city = value;
					this.SendPropertyChanged("city");
					this.OncityChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_country", DbType="NVarChar(50)")]
		public string country
		{
			get
			{
				return this._country;
			}
			set
			{
				if ((this._country != value))
				{
					this.OncountryChanging(value);
					this.SendPropertyChanging();
					this._country = value;
					this.SendPropertyChanged("country");
					this.OncountryChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_isPrivate", DbType="Bit NOT NULL")]
		public bool isPrivate
		{
			get
			{
				return this._isPrivate;
			}
			set
			{
				if ((this._isPrivate != value))
				{
					this.OnisPrivateChanging(value);
					this.SendPropertyChanging();
					this._isPrivate = value;
					this.SendPropertyChanged("isPrivate");
					this.OnisPrivateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_avatar", DbType="NVarChar(250)")]
		public string avatar
		{
			get
			{
				return this._avatar;
			}
			set
			{
				if ((this._avatar != value))
				{
					this.OnavatarChanging(value);
					this.SendPropertyChanging();
					this._avatar = value;
					this.SendPropertyChanged("avatar");
					this.OnavatarChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="User_Track", Storage="_Tracks", ThisKey="id", OtherKey="user_id")]
		public EntitySet<Track> Tracks
		{
			get
			{
				return this._Tracks;
			}
			set
			{
				this._Tracks.Assign(value);
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_Tracks(Track entity)
		{
			this.SendPropertyChanging();
			entity.User = this;
		}
		
		private void detach_Tracks(Track entity)
		{
			this.SendPropertyChanging();
			entity.User = null;
		}
	}
}
#pragma warning restore 1591
