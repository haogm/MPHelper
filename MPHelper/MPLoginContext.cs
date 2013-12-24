using System;
using System.Net;

namespace MPHelper
{
	internal class MPLoginContext
	{
		const int EXPIRES_MINUTES = 15;

		public string Token { get; set; }

		public CookieContainer LoginCookie { get; set; }

		private DateTime CreateDate { get; set; }


		private static object _locker = new object();
		private static MPLoginContext _current;

		public static MPLoginContext Current
		{
			get
			{
				if (_current != null && _current.LoginCookie != null && _current.CreateDate.AddMinutes(EXPIRES_MINUTES) > DateTime.Now)
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
