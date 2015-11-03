using System;
using System.Net;

namespace MPHelper
{
	internal class MpLoginContext
	{
		const int ExpirationMinutes = 3;

		public string Token { get; set; }

		public CookieContainer LoginCookie { get; set; }

		public string PluginToken { get; set; }

		public DateTime CreateDate { get; set; }

		public bool IsValid()
		{
			return LoginCookie != null && CreateDate.AddMinutes(ExpirationMinutes) > DateTime.Now;
		}
	}
}
