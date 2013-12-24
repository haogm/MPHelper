using System;
using System.Net;

namespace MPHelper
{
	internal class MPLoginContext
	{
		public string Token { get; set; }

		public CookieContainer LoginCookie { get; set; }

		public DateTime CreateDate { get; set; }

		static readonly int _expirationMinutes = 15;
		static readonly object _locker = new object();
		static MPLoginContext _current;

		public static MPLoginContext Current
		{
			get
			{
				if (_current != null && _current.LoginCookie != null 
					&& _current.CreateDate.AddMinutes(_expirationMinutes) > DateTime.Now)
				{
					return _current;
				}

				return null; 
			}
		}

		public static void SetLoginStatus(string token, CookieContainer cookie)
		{
			lock (_locker)
			{
				if (_current == null)
				{
					_current = new MPLoginContext();
				}

				_current.Token = token;
				_current.LoginCookie = cookie;
				_current.CreateDate = DateTime.Now;
			}
		}
	}
}
