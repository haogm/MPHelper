using System;
using System.Net;

namespace MPHelper
{
	internal class MpLoginContext
	{
		private readonly int _expirationMinutes;
		private DateTime _lastRefreshTime = new DateTime(2000, 1, 1);

		public string Token { get; private set; }

		public CookieContainer LoginCookie { get; private set; }

		public string PluginToken { get; private set; }


		public MpLoginContext(int expirationMinutes = 3)
		{
			_expirationMinutes = expirationMinutes;
		}

		public void Refresh(string token, CookieContainer cookie)
		{
			Token = token;
			LoginCookie = cookie;
			PluginToken = string.Empty;
			_lastRefreshTime = DateTime.Now;
		}

		public void SetPluginToken(string pluginToken)
		{
			PluginToken = pluginToken;
		}

		public bool IsValid()
		{
			return LoginCookie != null && _lastRefreshTime.AddMinutes(_expirationMinutes) > DateTime.Now;
		}
	}
}
