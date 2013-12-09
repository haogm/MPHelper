using System;
using System.Net;
using System.Web;

namespace WX.Tools
{
	using Hanger.Common;
	using Hanger.Utility;
	using Newtonsoft.Json;
	using System.Threading.Tasks;
	using WX.Tools.Results;
	using WX.Tools.Utility;

    public static class MPManager
    {
		const string MP_LOGIN_URL = "http://mp.weixin.qq.com/cgi-bin/login?lang=zh_CN";
		const string MP_MODIFYCONTACKS_URL = "https://mp.weixin.qq.com/cgi-bin/modifycontacts";
		static readonly string _mpAccount = AppSettingHelper.GetStringValue("MPAccount");
		static readonly string _mpPassword = AppSettingHelper.GetStringValue("MPPassword");

		static async Task<bool> LoginAsync()
		{
			if (MPLoginContext.Current != null)
			{
				return true;
			}

			var success = false;
			var postData = string.Format("username={0}&pwd={1}&imgcode=&f=json", 
				HttpUtility.UrlEncode(_mpAccount), 
				StringHelper.GetMd5(_mpPassword));

			var cookie = new CookieContainer();
			var resultJson = await MPRequestUtility.Post(MP_LOGIN_URL, postData, cookie);

			try
			{
				var result = JsonConvert.DeserializeObject<LoginResult>(resultJson);

				if (result != null && result.ErrMsg.Length > 0)
				{
					var token = result.ErrMsg.Split(new char[] { '&' })[2].Split(new char[] { '=' })[1];

					if (!string.IsNullOrWhiteSpace(token))
					{
						MPLoginContext.SetLoginStatus(token, cookie);

						success = true;
					}
				}
			}
			catch (Exception ex)
			{
				LocalLoggingService.Exception(ex);
			}

			return success;
		}

		public static async Task<string> GetLastedMessages()
		{
			if (MPLoginContext.Current == null)
			{
				var login = await LoginAsync();

				if (!login)
				{
					return null;
				}
			}

			var url = string.Format("https://mp.weixin.qq.com/cgi-bin/message?t=message/list&count=20&day=7&token={0}&lang=zh_CN", 
				MPLoginContext.Current.Token);

			var content = await MPRequestUtility.Get(url, MPLoginContext.Current.LoginCookie);

			return content;
		}

		public static async Task<string> GetFakeIdAsync(string openId)
		{
			return null;
		}

		public static async Task<bool> ChangeCategoryAsync(string fakeId, string cateId)
		{
			if (MPLoginContext.Current == null)
			{
				var login = await LoginAsync();

				if (!login)
				{
					return false;
				}
			}

			var postData = string.Format("contacttype={0}&tofakeidlist={1}&token={2}&lang=zh_CN&random={3}&f=json&ajax=1&action=modifycontacts&t=ajax-putinto-group",
				cateId, fakeId, MPLoginContext.Current.Token, "0.1234567890");

			var resultJson = await MPRequestUtility.Post(MP_MODIFYCONTACKS_URL, postData, MPLoginContext.Current.LoginCookie);

			try
			{
				var resultPackage = JsonConvert.DeserializeObject<ModifyContactResult>(resultJson);

				if (resultPackage != null && resultPackage.result.Count > 0 && resultPackage.result[0].fakeId == fakeId)
				{
					return true;
				}
			}
			catch (Exception ex)
			{
				LocalLoggingService.Exception(ex);
			}

			return false;
		}
    }
}
