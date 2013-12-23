using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace WX.Tools
{
	using Hanger.Common;
	using Hanger.Utility;
	using Newtonsoft.Json;
	using WX.Tools.DTOs;
	using WX.Tools.Results;
	using WX.Tools.Utility;

	public static class MPManager
	{
		static readonly string _mpAccount = AppSettingHelper.GetStringValue("MPAccount");
		static readonly string _mpPasswordMD5 = AppSettingHelper.GetStringValue("MPPasswordMD5");

		static async Task<bool> LoginAsync()
		{
			if (MPLoginContext.Current != null)
			{
				return true;
			}

			if (string.IsNullOrWhiteSpace(_mpAccount) || string.IsNullOrWhiteSpace(_mpPasswordMD5))
			{
				return false;
			}

			var success = false;
			var postData = string.Format("username={0}&pwd={1}&imgcode=&f=json",
				HttpUtility.UrlEncode(_mpAccount), _mpPasswordMD5);

			var cookie = new CookieContainer();
			var resultJson = await MPRequestUtility.PostAsync(MPAddresses.LOGIN_URL, postData, cookie);

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

		/// <summary>
		/// 获取所有用户消息列表
		/// </summary>
		/// <param name="count">消息条数</param>
		/// <param name="day">0：今天；1：昨天；以此类推，后台最多保存5天数据，默认全部消息</param>
		public static async Task<IList<MessageItem>> GetAllMessageListAsync(int count = 20, int day = 7)
		{
			if (MPLoginContext.Current == null)
			{
				var login = await LoginAsync();

				if (!login)
				{
					return null;
				}
			}

			if (count < 1 || count > 100)
			{
				count = 20;
			}

			if (day < 0 || day > 7)
			{
				day = 7;
			}

			var url = string.Format(MPAddresses.ALL_MESSAGE_LIST_URL_FORMAT,
				count, day, MPLoginContext.Current.Token);

			var htmlContent = await MPRequestUtility.GetAsync(url, MPLoginContext.Current.LoginCookie);

			if (!string.IsNullOrWhiteSpace(htmlContent))
			{
				var regex = new Regex(@"(?<=\({""msg_item"":\[).*(?=\]}\).msg_item)", RegexOptions.IgnoreCase);
				var match = regex.Match(htmlContent);

				if (match.Groups.Count > 0)
				{
					var listJson = string.Format("[{0}]", match.Groups[0].Value);

					try
					{
						return listJson.JsonToObject<List<MessageItem>>();
					}
					catch (Exception ex)
					{
						LocalLoggingService.Exception(ex);
					}
				}
			}

			return null;
		}

		/// <summary>
		/// 获取星标消息列表
		/// </summary>
		/// <param name="count">消息条数</param>
		public static async Task<IList<MessageItem>> GetStarMessageListAsync(int count = 20)
		{
			if (MPLoginContext.Current == null)
			{
				var login = await LoginAsync();

				if (!login)
				{
					return null;
				}
			}

			if (count < 1 || count > 100)
			{
				count = 20;
			}

			var url = string.Format(MPAddresses.STAR_MESSAGE_LIST_URL_FORMAT,
				count, MPLoginContext.Current.Token);

			var htmlContent = await MPRequestUtility.GetAsync(url, MPLoginContext.Current.LoginCookie);

			if (!string.IsNullOrWhiteSpace(htmlContent))
			{
				var regex = new Regex(@"(?<=\({""msg_item"":\[).*(?=\]}\).msg_item)", RegexOptions.IgnoreCase);
				var match = regex.Match(htmlContent);

				if (match.Groups.Count > 0)
				{
					var listJson = string.Format("[{0}]", match.Groups[0].Value);

					try
					{
						return listJson.JsonToObject<List<MessageItem>>();
					}
					catch (Exception ex)
					{
						LocalLoggingService.Exception(ex);
					}
				}
			}

			return null;
		}

		/// <summary>
		/// 获取单个用户对话消息列表
		/// </summary>
		/// <param name="fakeId">用户FakeId</param>
		public static async Task<IList<MessageItem>> GetSingleSendMessageListAsync(string fakeId)
		{
			if (MPLoginContext.Current == null)
			{
				var login = await LoginAsync();

				if (!login)
				{
					return null;
				}
			}

			var url = string.Format(MPAddresses.SINGLE_SEND_MESSAGE_LIST_URL_FORMAT,
				fakeId, MPLoginContext.Current.Token);

			var htmlContent = await MPRequestUtility.GetAsync(url, MPLoginContext.Current.LoginCookie);

			if (!string.IsNullOrWhiteSpace(htmlContent))
			{
				var regex = new Regex(@"(?<=""msg_items"":{""msg_item"":\[).*(?=\]}})", RegexOptions.IgnoreCase);
				var match = regex.Match(htmlContent);

				if (match.Groups.Count > 0)
				{
					var listJson = string.Format("[{0}]", match.Groups[0].Value);

					try
					{
						return listJson.JsonToObject<List<MessageItem>>();
					}
					catch (Exception ex)
					{
						LocalLoggingService.Exception(ex);
					}
				}
			}

			return null;
		}

		/// <summary>
		/// 更改用户分组
		/// </summary>
		/// <param name="fakeId">用户FakeId</param>
		/// <param name="cateId">分组ID</param>
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

			var resultJson = await MPRequestUtility.PostAsync(MPAddresses.MODIFY_CATEGORY_URL, postData, MPLoginContext.Current.LoginCookie);

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

		/// <summary>
		/// 发送信息，目前只支持文字消息
		/// </summary>
		/// <param name="fakeId">用户FakeId</param>
		/// <param name="message">文字消息</param>
		public static async Task<bool> SendMessageAsync(string fakeId, string message)
		{
			if (MPLoginContext.Current == null)
			{
				var login = await LoginAsync();

				if (!login)
				{
					return false;
				}
			}

			var postData = string.Format("type={0}&content={1}&tofakeid={2}&imgcode={3}&token={4}&lang=zh_CN&random={5}&f=json&ajax=1&t=ajax-response",
				"1", message, fakeId, string.Empty, MPLoginContext.Current.Token, "0.1234567890");

			var resultJson = await MPRequestUtility.PostAsync(MPAddresses.SEND_MESSAGE_URL, postData, MPLoginContext.Current.LoginCookie);

			try
			{
				var resultPackage = JsonConvert.DeserializeObject<SendMessageResult>(resultJson);

				if (resultPackage != null && resultPackage.base_resp != null
					&& !string.IsNullOrWhiteSpace(resultPackage.base_resp.err_msg) && resultPackage.base_resp.err_msg.ToLower().Equals("ok"))
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

		/// <summary>
		/// 获取用户信息
		/// </summary>
		/// <param name="fakeId">用户FakeId</param>
		public static async Task<ContactInfo> GetContactInfoAsync(string fakeId)
		{
			if (MPLoginContext.Current == null)
			{
				var login = await LoginAsync();

				if (!login)
				{
					return null;
				}
			}

			var postData = string.Format("fakeid={0}&token={1}&lang=zh_CN&random={2}&f=json&ajax=1&t=ajax-getcontactinfo",
				fakeId, MPLoginContext.Current.Token, "0.1234567890");

			var resultJson = await MPRequestUtility.PostAsync(MPAddresses.GET_USERINFO_URL, postData, MPLoginContext.Current.LoginCookie);

			try
			{
				var resultPackage = JsonConvert.DeserializeObject<GetContactResult>(resultJson);

				if (resultPackage != null)
				{
					return resultPackage.contact_info;
				}
			}
			catch (Exception ex)
			{
				LocalLoggingService.Exception(ex);
			}

			return null;
		}
	}
}
