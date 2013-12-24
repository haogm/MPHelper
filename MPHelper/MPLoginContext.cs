using System;
using System.Net;

namespace MPHelper
{
	internal class MPLoginContext
	{
		const int EXPIRATION_MINUTES = 15;

		public string Token { get; set; }

		public CookieContainer LoginCookie { get; set; }

		public DateTime CreateDate { get; set; }


		public bool IsValid()
		{
			if (this.LoginCookie != null && this.CreateDate.AddMinutes(EXPIRATION_MINUTES) > DateTime.Now)
			{
				return true;
			}

			return false;
		}
	}
}
