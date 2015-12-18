using System;
using System.Net;

namespace MPHelper
{
	internal class MpLoginContext
	{
		private const int ExpiredMinutes = 5;
		private DateTime _lastRefreshTime = new DateTime(2000, 1, 1);

		public string Token { get; private set; }

		public CookieContainer LoginCookie { get; private set; }

		public void Refresh(string token, CookieContainer cookie)
		{
			Token = token;
			LoginCookie = cookie;

			_lastRefreshTime = DateTime.Now;
		}

		public bool IsValid()
		{
			return !string.IsNullOrWhiteSpace(Token) && LoginCookie != null
			       && _lastRefreshTime.AddMinutes(ExpiredMinutes) > DateTime.Now;
		}
	}
}
