using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using MPHelper.DTOs;
using MPHelper.Results;
using MPHelper.Utility;

namespace MPHelper
{
	/// <summary>
	/// 操作集合类
	/// </summary>
	public class MPManager
	{
		private static readonly object LoginLocker = new object();
		private static readonly Dictionary<string, MPLoginContext> LoginContext = new Dictionary<string, MPLoginContext>();

		private readonly string _mpAccount;
		private readonly string _mpPasswordMd5;

		/// <summary>
		/// MPManager
		/// </summary>
		/// <param name="mpAccount">公众账号登录用户名</param>
		/// <param name="mpPasswordMd5">公众账号登录密码MD5值</param>
		public MPManager(string mpAccount, string mpPasswordMd5)
		{
			if (string.IsNullOrWhiteSpace(mpAccount))
				throw new ArgumentNullException("mpAccount");

			if (string.IsNullOrWhiteSpace(mpPasswordMd5))
				throw new ArgumentNullException("mpPasswordMd5");

			_mpAccount = mpAccount;
			_mpPasswordMd5 = mpPasswordMd5;
		}

		/// <summary>
		/// 模拟后台登录
		/// </summary>
		private async Task<bool> LoginAsync()
		{
			if (LoginContext.ContainsKey(_mpAccount) && LoginContext[_mpAccount].IsValid())
				return true;

			var success = false;
			var postData = string.Format("username={0}&pwd={1}&imgcode=&f=json",
				HttpUtility.UrlEncode(_mpAccount), _mpPasswordMd5);

			var cookie = new CookieContainer();
			var resultJson = await MPRequestUtility.PostAsync(MPAddresses.LoginUrl, postData, cookie);
			var resultPackage = JsonHelper.Deserialize<LoginResult>(resultJson);

			if (resultPackage != null && resultPackage.base_resp != null && resultPackage.base_resp.ret == 0)
			{
				var token = resultPackage.redirect_url.Split('&')[2].Split('=')[1];

				if (!string.IsNullOrWhiteSpace(token))
				{
					if (!LoginContext.ContainsKey(_mpAccount))
					{
						lock (LoginLocker)
						{
							if (!LoginContext.ContainsKey(_mpAccount))
								LoginContext.Add(_mpAccount, new MPLoginContext());
						}
					}

					LoginContext[_mpAccount].Token = token;
					LoginContext[_mpAccount].LoginCookie = cookie;
					LoginContext[_mpAccount].CreateDate = DateTime.Now;

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
		public async Task<IList<MessageItem>> GetAllMessageListAsync(int count = 20, int day = 7)
		{
			if (!await LoginAsync())
				return null;

			if (count < 1 || count > 100)
				count = 20;

			if (day < 0 || day > 7)
				day = 7;

			var url = string.Format(MPAddresses.AllMessageListUrlFormat,
				count, day, LoginContext[_mpAccount].Token);

			return await GetMessageListAsync(url);
		}

		/// <summary>
		/// 根据关键字检索消息列表
		/// </summary>
		/// <param name="keyword">关键字</param>
		/// <param name="count">消息条数</param>
		public async Task<IList<MessageItem>> GetMessageListByKeywordAsync(string keyword, int count = 20)
		{
			if (!await LoginAsync())
				return null;

			if (string.IsNullOrWhiteSpace(keyword))
				return null;

			if (count < 1 || count > 100)
				count = 20;

			var url = string.Format(MPAddresses.KeywordMessageListUrlFormat,
				keyword, count, LoginContext[_mpAccount].Token);

			return await GetMessageListAsync(url);
		}

		/// <summary>
		/// 获取星标消息列表
		/// </summary>
		/// <param name="count">消息条数</param>
		public async Task<IList<MessageItem>> GetStarMessageListAsync(int count = 20)
		{
			if (!await LoginAsync())
				return null;

			if (count < 1 || count > 100)
				count = 20;

			var url = string.Format(MPAddresses.StarMessageListUrlFormat,
				count, LoginContext[_mpAccount].Token);

			return await GetMessageListAsync(url);
		}

		/// <summary>
		/// 设置/取消星标消息
		/// </summary>
		/// <param name="messageId">消息ID</param>
		/// <param name="isStar">是否为星标</param>
		public async Task<bool> SetStarMessageAsync(string messageId, bool isStar)
		{
			if (!await LoginAsync())
				return false;

			var postData = string.Format("msgid={0}&value={1}&token={2}&lang=zh_CN&random=0.1234567890&f=json&ajax=1&t=ajax-setstarmessage",
				messageId, isStar ? "1" : "0", LoginContext[_mpAccount].Token);

			var resultJson = await MPRequestUtility.PostAsync(MPAddresses.SetStartMessageUrl, postData, LoginContext[_mpAccount].LoginCookie);
			var resultPackage = JsonHelper.Deserialize<CommonExecuteResult>(resultJson);

			return resultPackage != null && resultPackage.ret == 0;
		}

		/// <summary>
		/// 获取单个用户对话消息列表
		/// </summary>
		/// <param name="fakeId">用户FakeId</param>
		public async Task<IList<MessageItem>> GetSingleSendMessageListAsync(string fakeId)
		{
			if (!await LoginAsync())
				return null;

			var url = string.Format(MPAddresses.SingleSendMessageListUrlFormat,
				fakeId, LoginContext[_mpAccount].Token);

			var htmlContent = await MPRequestUtility.GetAsync(url, LoginContext[_mpAccount].LoginCookie);

			if (!string.IsNullOrWhiteSpace(htmlContent))
			{
				var regex = new Regex(@"(?<=""msg_items"":{""msg_item"":\[).*(?=\]}})", RegexOptions.IgnoreCase);
				var match = regex.Match(htmlContent);

				if (match.Groups.Count > 0)
					return JsonHelper.Deserialize<List<MessageItem>>(string.Format("[{0}]", match.Groups[0].Value));
			}

			return null;
		}

		/// <summary>
		/// 更改用户分组（0：未分组； 1：黑名单； 2：星标组）
		/// </summary>
		/// <param name="fakeId">用户FakeId</param>
		/// <param name="cateId">分组ID</param>
		public async Task<bool> ChangeCategoryAsync(string fakeId, string cateId)
		{
			if (!await LoginAsync())
				return false;

			var postData = string.Format("contacttype={0}&tofakeidlist={1}&token={2}&lang=zh_CN&random=0.1234567890&f=json&ajax=1&action=modifycontacts&t=ajax-putinto-group",
				cateId, fakeId, LoginContext[_mpAccount].Token);

			var resultJson = await MPRequestUtility.PostAsync(MPAddresses.ModifyCategoryUrl, postData, LoginContext[_mpAccount].LoginCookie);
			var resultPackage = JsonHelper.Deserialize<ModifyContactResult>(resultJson);

			return resultPackage != null && resultPackage.ret == 0;
		}

		/// <summary>
		/// 获取用户信息
		/// </summary>
		/// <param name="fakeId">用户FakeId</param>
		public async Task<ContactInfo> GetContactInfoAsync(string fakeId)
		{
			if (!await LoginAsync())
				return null;

			var postData = string.Format("fakeid={0}&token={1}&lang=zh_CN&random=0.1234567890&f=json&ajax=1&t=ajax-getcontactinfo",
				fakeId, LoginContext[_mpAccount].Token);

			var resultJson = await MPRequestUtility.PostAsync(MPAddresses.GetContactinfoUrl, postData, LoginContext[_mpAccount].LoginCookie);
			var resultPackage = JsonHelper.Deserialize<GetContactResult>(resultJson);

			return resultPackage != null ? resultPackage.contact_info : null;
		}

		/// <summary>
		/// 单用户消息发送
		/// </summary>
		/// <param name="fakeId">用户FakeId</param>
		/// <param name="type">消息类型</param>
		/// <param name="value">
		/// 文字消息：文字内容; 
		/// 图片、音频、视频消息：文件ID; 
		/// 图文消息：消息ID; 
		/// </param>
		public async Task<bool> SingleSendMessageAsync(string fakeId, MPMessageType type, string value)
		{
			if (!await LoginAsync())
				return false;

			var postData = new StringBuilder();

			postData.AppendFormat("type={0}&tofakeid={1}&&token={2}&lang=zh_CN&random=0.1234567890&f=json&ajax=1&t=ajax-response",
				(int)type, fakeId, LoginContext[_mpAccount].Token);

			switch (type)
			{
				case MPMessageType.Text:
					if (string.IsNullOrWhiteSpace(value))
						throw new ArgumentNullException("value", "文字消息内容为空");

					postData.AppendFormat("&content={0}", HttpUtility.UrlEncode(value, Encoding.UTF8));
					break;
				case MPMessageType.Image:
				case MPMessageType.Audio:
				case MPMessageType.Video:
					if (string.IsNullOrWhiteSpace(value))
						throw new ArgumentNullException("value", "文件ID为空");

					postData.AppendFormat("&file_id={0}&fileid={0}", value);
					break;
				case MPMessageType.AppMsg:
					if (string.IsNullOrWhiteSpace(value))
						throw new ArgumentNullException("value", "图文消息ID为空");

					postData.AppendFormat("&app_id={0}&appmsgid={0}", value);
					break;
			}

			var resultJson = await MPRequestUtility.PostAsync(
				MPAddresses.SingleSendMessageUrl, postData.ToString(), LoginContext[_mpAccount].LoginCookie);
			var resultPackage = JsonHelper.Deserialize<SendMessageResult>(resultJson);

			return resultPackage != null && resultPackage.base_resp != null && resultPackage.base_resp.ret == 0;
		}

		/// <summary>
		/// 群组消息推送
		/// </summary>
		/// <param name="type">消息类型</param>
		/// <param name="value">
		/// 文字消息：文字内容; 
		/// 图片、音频、视频消息：文件ID; 
		/// 图文消息：消息ID; 
		/// </param>
		/// <param name="groupId">分组ID</param>
		/// <param name="gender">性别（0：全部； 1：男； 2：女）</param>
		/// <param name="country">国家</param>
		/// <param name="province">省</param>
		/// <param name="city">市</param>
		/// <returns></returns>
		public async Task<bool> MassSendMessageAsync(MPMessageType type, string value, string groupId = "-1", int gender = 0,
			string country = null, string province = null, string city = null)
		{
			if (!await LoginAsync())
				return false;

			var postData = new StringBuilder();

			postData.AppendFormat(
				"type={0}&groupid={1}&sex={2}&country={3}&province={4}&city={5}&token={6}&synctxweibo=0&synctxnews=0&imgcode=&lang=zh_CN&random=0.1234567890&f=json&ajax=1&t=ajax-response",
				(int) type,
				string.IsNullOrWhiteSpace(groupId) ? "-1" : groupId,
				gender,
				string.IsNullOrWhiteSpace(country) ? string.Empty : HttpUtility.UrlEncode(country, Encoding.UTF8),
				string.IsNullOrWhiteSpace(province) ? string.Empty : HttpUtility.UrlEncode(province, Encoding.UTF8),
				string.IsNullOrWhiteSpace(city) ? string.Empty : HttpUtility.UrlEncode(city, Encoding.UTF8),
				LoginContext[_mpAccount].Token);

			switch (type)
			{
				case MPMessageType.Text:
					if (string.IsNullOrWhiteSpace(value))
						throw new ArgumentNullException("value", "文字消息内容为空");

					postData.AppendFormat("&content={0}", HttpUtility.UrlEncode(value, Encoding.UTF8));
					break;
				case MPMessageType.Image:
				case MPMessageType.Audio:
				case MPMessageType.Video:
					if (string.IsNullOrWhiteSpace(value))
						throw new ArgumentNullException("value", "文件ID为空");

					postData.AppendFormat("&fileid={0}", value);
					break;
				case MPMessageType.AppMsg:
					if (string.IsNullOrWhiteSpace(value))
						throw new ArgumentNullException("value", "图文消息ID为空");

					postData.AppendFormat("&appmsgid={0}", value);
					break;
			}

			var resultJson = await MPRequestUtility.PostAsync(
				MPAddresses.MassSendMessageUrl, postData.ToString(), LoginContext[_mpAccount].LoginCookie);
			var resultPackage = JsonHelper.Deserialize<CommonExecuteResult>(resultJson);

			return resultPackage != null && resultPackage.ret == 0;
		}

		/// <summary>
		/// 获取后台文件（音频、图片）
		/// </summary>
		/// <param name="msgId">消息ID</param>
		/// <returns></returns>
		public byte[] GetDonwloadFileBytes(int msgId)
		{
			if (!LoginAsync().Result)
				return null;

			var url = string.Format(MPAddresses.DownloadFileUrlFormat, msgId, LoginContext[_mpAccount].Token);

			return MPRequestUtility.GetDonwloadFileBytes(url, LoginContext[_mpAccount].LoginCookie);
		}

		#region private

		private async Task<IList<MessageItem>> GetMessageListAsync(string url)
		{
			var htmlContent = await MPRequestUtility.GetAsync(url, LoginContext[_mpAccount].LoginCookie);

			if (!string.IsNullOrWhiteSpace(htmlContent))
			{
				var regex = new Regex(@"(?<=\({""msg_item"":\[).*(?=\]}\).msg_item)", RegexOptions.IgnoreCase);
				var match = regex.Match(htmlContent);

				if (match.Groups.Count > 0)
					return JsonHelper.Deserialize<List<MessageItem>>(string.Format("[{0}]", match.Groups[0].Value));
			}

			return null;
		}

		#endregion
	}
}
