using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace MPHelper
{
	using MPHelper.DTOs;
	using MPHelper.Results;
	using MPHelper.Utility;
	using Newtonsoft.Json;

	/// <summary>
	/// 操作集合类
	/// </summary>
	public static class MPManager
	{
		static readonly string _mpAccount = ConfigurationManager.AppSettings["MPAccount"];
		static readonly string _mpPasswordMD5 = ConfigurationManager.AppSettings["MPPasswordMD5"];

		/// <summary>
		/// 模拟后台登录
		/// </summary>
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
			var resultPackage = JsonConvert.DeserializeObject<LoginResult>(resultJson);

			if (resultPackage != null && resultPackage.ErrMsg.Length > 0)
			{
				var token = resultPackage.ErrMsg.Split(new char[] { '&' })[2].Split(new char[] { '=' })[1];

				if (!string.IsNullOrWhiteSpace(token))
				{
					MPLoginContext.SetLoginStatus(token, cookie);

					success = true;
				}
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

			return await GetMessageListAsync(url);
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

			return await GetMessageListAsync(url);
		}

		/// <summary>
		/// 设置/取消星标消息
		/// </summary>
		/// <param name="messageId">消息ID</param>
		/// <param name="isStar">是否为星标</param>
		public static async Task<bool> SetStarMessageAsync(string messageId, bool isStar)
		{
			if (MPLoginContext.Current == null)
			{
				var login = await LoginAsync();

				if (!login)
				{
					return false;
				}
			}

			var postData = string.Format("msgid={0}&value={1}&token={2}&lang=zh_CN&random={3}&f=json&ajax=1&t=ajax-setstarmessage",
				messageId, isStar ? "1" : "0", MPLoginContext.Current.Token, "0.1234567890");

			var resultJson = await MPRequestUtility.PostAsync(MPAddresses.SET_START_MESSAGE_URL, postData, MPLoginContext.Current.LoginCookie);
			var resultPackage = JsonConvert.DeserializeObject<CommonExecuteResult>(resultJson);

			if (resultPackage != null && resultPackage.msg.Equals("sys ok"))
			{
				return true;
			}

			return false;
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

					return JsonHelper.Deserialize<List<MessageItem>>(listJson);
				}
			}

			return null;
		}

		/// <summary>
		/// 更改用户分组（0：未分组； 1：黑名单； 2：星标组）
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
			var resultPackage = JsonConvert.DeserializeObject<ModifyContactResult>(resultJson);

			if (resultPackage != null && resultPackage.result.Count > 0 && resultPackage.result[0].fakeId == fakeId)
			{
				return true;
			}
			
			return false;
		}

		/// <summary>
		/// 单用户消息发送
		/// </summary>
		/// <param name="fakeId">用户FakeId</param>
		/// <param name="type">消息类型</param>
		/// <param name="content">文字消息内容</param>
		/// <param name="fileId">图片、音频、视频消息文件ID</param>
		/// <param name="appMsgId">图文消息ID</param>
		public static async Task<bool> SingleSendMessageAsync(string fakeId, MPMessageType type, string content = null, string fileId = null, string appMsgId = null)
		{
			if (MPLoginContext.Current == null)
			{
				var login = await LoginAsync();

				if (!login)
				{
					return false;
				}
			}

			var postData = new StringBuilder();

			postData.AppendFormat("type={0}&tofakeid={1}&&token={2}&lang=zh_CN&random=0.1234567890&f=json&ajax=1&t=ajax-response",
				(int)type, fakeId, MPLoginContext.Current.Token, "0.1234567890");

			switch (type)
			{
				case MPMessageType.Text:
					if (string.IsNullOrWhiteSpace(content))
					{
						throw new ArgumentNullException("content", "文字消息内容为空");
					}

					postData.AppendFormat("&content={0}", content);
					break;
				case MPMessageType.Image:
				case MPMessageType.Audio:
				case MPMessageType.Video:
					if (string.IsNullOrWhiteSpace(fileId))
					{
						throw new ArgumentNullException("fileId", "文件ID为空");
					}

					postData.AppendFormat("&file_id={0}&fileid={0}", fileId);
					break;
				case MPMessageType.AppMsg:
					if (string.IsNullOrWhiteSpace(appMsgId))
					{
						throw new ArgumentNullException("appMsgId", "图文消息ID为空");
					}

					postData.AppendFormat("&app_id={0}&appmsgid={0}", appMsgId);
					break;
				default:
					break;
			}

			var resultJson = await MPRequestUtility.PostAsync(MPAddresses.SINGLE_SEND_MESSAGE_URL, postData.ToString(), MPLoginContext.Current.LoginCookie);
			var resultPackage = JsonConvert.DeserializeObject<SendMessageResult>(resultJson);

			if (resultPackage != null && resultPackage.base_resp != null
				&& !string.IsNullOrWhiteSpace(resultPackage.base_resp.err_msg) && resultPackage.base_resp.err_msg.ToLower().Equals("ok"))
			{
				return true;
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

			var resultJson = await MPRequestUtility.PostAsync(MPAddresses.GET_CONTACTINFO_URL, postData, MPLoginContext.Current.LoginCookie);
			var resultPackage = JsonConvert.DeserializeObject<GetContactResult>(resultJson);

			if (resultPackage != null)
			{
				return resultPackage.contact_info;
			}

			return null;
		}

		#region private

		static async Task<IList<MessageItem>> GetMessageListAsync(string url)
		{
			var htmlContent = await MPRequestUtility.GetAsync(url, MPLoginContext.Current.LoginCookie);

			if (!string.IsNullOrWhiteSpace(htmlContent))
			{
				var regex = new Regex(@"(?<=\({""msg_item"":\[).*(?=\]}\).msg_item)", RegexOptions.IgnoreCase);
				var match = regex.Match(htmlContent);

				if (match.Groups.Count > 0)
				{
					var listJson = string.Format("[{0}]", match.Groups[0].Value);

					return JsonHelper.Deserialize<List<MessageItem>>(listJson);
				}
			}

			return null;
		}

		#endregion
	}
}
