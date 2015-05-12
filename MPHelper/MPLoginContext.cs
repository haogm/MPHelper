using System;
using System.Net;

namespace MPHelper
{
	internal class MPLoginContext
	{
		const int ExpirationMinutes = 15;

		public string Token { get; set; }

		public CookieContainer LoginCookie { get; set; }

		public DateTime CreateDate { get; set; }

		public bool IsValid()
		{
			return LoginCookie != null && CreateDate.AddMinutes(ExpirationMinutes) > DateTime.Now;
		}
	}
}
