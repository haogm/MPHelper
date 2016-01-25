using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using MPHelper.Dtos;
using MPHelper.InternalResults;
using MPHelper.InternalUtilities;

namespace MPHelper
{
	/// <summary>
	/// 操作集合类
	/// </summary>
	public class MpManager
	{
		private static readonly Regex TokenRegex = new Regex(@"(?:^|\?|&)token=(\d*)(?:&|$)");
		private static readonly Dictionary<string, MpManager> Instances = new Dictionary<string, MpManager>();

		private readonly object _loginLocker = new object();
		private readonly string _userName;
		private readonly string _pwdMd5;
		private readonly MpLoginContext _loginContext;

		/// <summary>
		/// Get MPManager Instance
		/// </summary>
		/// <param name="userName">公众账号登录用户名</param>
		/// <param name="pwdMd5">公众账号登录密码MD5值</param>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public static MpManager GetInstance(string userName, string pwdMd5)
		{
			if (Instances.ContainsKey(userName))
				return Instances[userName];

			var instance = new MpManager(userName, pwdMd5);

			Instances.Add(userName, instance);

			return instance;
		}

		/// <summary>
		/// MPManager
		/// </summary>
		/// <param name="userName">公众账号登录用户名</param>
		/// <param name="pwdMd5">公众账号登录密码MD5值</param>
		private MpManager(string userName, string pwdMd5)
		{
			if (string.IsNullOrWhiteSpace(userName))
				throw new ArgumentNullException("userName");

			if (string.IsNullOrWhiteSpace(pwdMd5))
				throw new ArgumentNullException("pwdMd5");

			_userName = userName;
			_pwdMd5 = pwdMd5;
			_loginContext = new MpLoginContext();
		}

		/// <summary>
		/// 预热
		/// </summary>
		public MpManager Preheat()
		{
			InternalLogin();

			return this;
		}

		/// <summary>
		/// 获取所有用户消息列表
		/// </summary>
		/// <param name="count">消息条数</param>
		/// <param name="day">0：今天；1：昨天；以此类推，后台最多保存5天数据，默认全部消息</param>
		public IEnumerable<MessageItem> GetAllMessageList(int count = 20, int day = 7)
		{
			if (!InternalLogin())
				return null;

			if (count < 1 || count > 100)
				count = 20;

			if (day < 0 || day > 7)
				day = 7;

			var url = string.Format(MpAddresses.AllMessageListUrlFormat, count, day, _loginContext.Token);

			return InternalGetMessageList(url);
		}

		/// <summary>
		/// 根据关键字检索消息列表
		/// </summary>
		/// <param name="keyword">关键字</param>
		/// <param name="count">消息条数</param>
		public IEnumerable<MessageItem> GetMessageListByKeyword(string keyword, int count = 20)
		{
			if (!InternalLogin())
				return null;

			if (string.IsNullOrWhiteSpace(keyword))
				return null;

			if (count < 1 || count > 100)
				count = 20;

			var url = string.Format(MpAddresses.KeywordMessageListUrlFormat, keyword, count, _loginContext.Token);

			return InternalGetMessageList(url);
		}

		/// <summary>
		/// 获取星标消息列表
		/// </summary>
		/// <param name="count">消息条数</param>
		public IEnumerable<MessageItem> GetStarMessageList(int count = 20)
		{
			if (!InternalLogin())
				return null;

			if (count < 1 || count > 100)
				count = 20;

			var url = string.Format(MpAddresses.StarMessageListUrlFormat, count, _loginContext.Token);

			return InternalGetMessageList(url);
		}

		/// <summary>
		/// 设置/取消星标消息
		/// </summary>
		/// <param name="messageId">消息ID</param>
		/// <param name="isStar">是否为星标</param>
		public Task<bool> SetStarMessageAsync(string messageId, bool isStar)
		{
			return Task.Factory.StartNew(() =>
			{
				if (!InternalLogin())
					return false;

				var postData = string.Format("msgid={0}&value={1}&token={2}&lang=zh_CN&random=0.1234567890&f=json&ajax=1&t=ajax-setstarmessage",
					messageId, isStar ? "1" : "0", _loginContext.Token);

				var resultJson = RequestHelper.Post(MpAddresses.SetStartMessageUrl, postData, _loginContext.LoginCookie);
				var resultPackage = JsonHelper.Deserialize<CommonExecuteResult>(resultJson);

				return resultPackage != null && resultPackage.ret == 0;
			});
		}

		/// <summary>
		/// 获取单个用户对话消息列表
		/// </summary>
		/// <param name="openId">用户OpenId</param>
		public IEnumerable<MessageItem> GetSingleSendMessageList(string openId)
		{
			if (!InternalLogin())
				return null;

			var url = string.Format(MpAddresses.SingleSendMessageListUrlFormat,
				openId, _loginContext.Token);

			var htmlContent = RequestHelper.Get(url, _loginContext.LoginCookie);

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
		/// <param name="openId">用户OpenId</param>
		/// <param name="cateId">分组ID</param>
		public Task<bool> ChangeCategoryAsync(string openId, string cateId)
		{
			return Task.Factory.StartNew(() =>
			{
				if (!InternalLogin())
					return false;

				var postData = string.Format("contacttype={0}&tofakeidlist={1}&token={2}&lang=zh_CN&random=0.1234567890&f=json&ajax=1&action=modifycontacts&t=ajax-putinto-group",
					cateId, openId, _loginContext.Token);

				var resultJson = RequestHelper.Post(MpAddresses.ModifyCategoryUrl, postData, _loginContext.LoginCookie);
				var resultPackage = JsonHelper.Deserialize<ModifyContactResult>(resultJson);

				return resultPackage != null && resultPackage.ret == 0;
			});
		}

		/// <summary>
		/// 获取用户信息
		/// </summary>
		/// <param name="openId">用户OpenId</param>
		public ContactInfo GetContactInfo(string openId)
		{
			if (!InternalLogin())
				return null;

			var postData = string.Format("fakeid={0}&token={1}&lang=zh_CN&random=0.1234567890&f=json&ajax=1&t=ajax-getcontactinfo",
				openId, _loginContext.Token);

			var resultJson = RequestHelper.Post(MpAddresses.GetContactinfoUrl, postData, _loginContext.LoginCookie);
			var resultPackage = JsonHelper.Deserialize<GetContactResult>(resultJson);

			return resultPackage != null ? resultPackage.contact_info : null;
		}

		/// <summary>
		/// 单用户消息发送
		/// </summary>
		/// <param name="openId">用户OpenId</param>
		/// <param name="type">消息类型</param>
		/// <param name="value">
		/// 文字消息：文字内容; 
		/// 图片、音频、视频消息：文件ID; 
		/// 图文消息：消息ID; 
		/// </param>
		public Task<bool> SingleSendMessageAsync(string openId, MpMessageType type, string value)
		{
			return Task.Factory.StartNew(() =>
			{
				if (!InternalLogin())
					return false;

				var postData = new StringBuilder();

				postData.AppendFormat(
					"type={0}&tofakeid={1}&&token={2}&lang=zh_CN&random=0.1234567890&f=json&ajax=1&t=ajax-response",
					(int) type, openId, _loginContext.Token);

				switch (type)
				{
					case MpMessageType.Text:
						if (string.IsNullOrWhiteSpace(value))
							throw new ArgumentNullException("value", "文字消息内容为空");

						postData.AppendFormat("&content={0}", HttpUtility.UrlEncode(value, Encoding.UTF8));
						break;
					case MpMessageType.Image:
					case MpMessageType.Audio:
					case MpMessageType.Video:
						if (string.IsNullOrWhiteSpace(value))
							throw new ArgumentNullException("value", "文件ID为空");

						postData.AppendFormat("&file_id={0}&fileid={0}", value);
						break;
					case MpMessageType.AppMsg:
						if (string.IsNullOrWhiteSpace(value))
							throw new ArgumentNullException("value", "图文消息ID为空");

						postData.AppendFormat("&app_id={0}&appmsgid={0}", value);
						break;
				}

				var resultJson = RequestHelper.Post(MpAddresses.SingleSendMessageUrl, postData.ToString(), _loginContext.LoginCookie);
				var resultPackage = JsonHelper.Deserialize<SendMessageResult>(resultJson);

				return resultPackage != null && resultPackage.base_resp != null && resultPackage.base_resp.ret == 0;
			});
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
		public Task<bool> MassSendMessageAsync(MpMessageType type, string value, string groupId = "-1", int gender = 0,
			string country = null, string province = null, string city = null)
		{
			return Task.Factory.StartNew(() =>
			{
				if (!InternalLogin())
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
					_loginContext.Token);

				switch (type)
				{
					case MpMessageType.Text:
						if (string.IsNullOrWhiteSpace(value))
							throw new ArgumentNullException("value", "文字消息内容为空");

						postData.AppendFormat("&content={0}", HttpUtility.UrlEncode(value, Encoding.UTF8));
						break;
					case MpMessageType.Image:
					case MpMessageType.Audio:
					case MpMessageType.Video:
						if (string.IsNullOrWhiteSpace(value))
							throw new ArgumentNullException("value", "文件ID为空");

						postData.AppendFormat("&fileid={0}", value);
						break;
					case MpMessageType.AppMsg:
						if (string.IsNullOrWhiteSpace(value))
							throw new ArgumentNullException("value", "图文消息ID为空");

						postData.AppendFormat("&appmsgid={0}", value);
						break;
				}

				var resultJson = RequestHelper.Post(MpAddresses.MassSendMessageUrl, postData.ToString(), _loginContext.LoginCookie);
				var resultPackage = JsonHelper.Deserialize<CommonExecuteResult>(resultJson);

				return resultPackage != null && resultPackage.ret == 0;
			});
		}

		/// <summary>
		/// 获取后台文件（音频、图片）
		/// </summary>
		/// <param name="msgId">消息ID</param>
		public byte[] GetDonwloadFileBytes(int msgId)
		{
			if (!InternalLogin())
				return null;

			var url = string.Format(MpAddresses.DownloadFileUrlFormat, msgId, _loginContext.Token);

			return RequestHelper.GetDonwloadFileBytes(url, _loginContext.LoginCookie);
		}

		#region private

		/// <summary>
		/// 模拟后台登录
		/// </summary>
		private bool InternalLogin()
		{
			if (_loginContext.IsValid())
				return true;

			lock (_loginLocker)
			{
				if (_loginContext.IsValid())
					return true;

				var postData = string.Format("username={0}&pwd={1}&imgcode=&f=json", HttpUtility.UrlEncode(_userName), _pwdMd5);
				var cookie = new CookieContainer();
				var resultJson = RequestHelper.Post(MpAddresses.LoginUrl, postData, cookie);
				var result = JsonHelper.Deserialize<LoginResult>(resultJson);

				if (result != null && result.base_resp != null && result.base_resp.ret == 0)
				{
					var tokenMatches = TokenRegex.Match(result.redirect_url);

					if (tokenMatches.Groups.Count > 1 && !string.IsNullOrWhiteSpace(tokenMatches.Groups[1].Value))
					{
						_loginContext.Refresh(tokenMatches.Groups[1].Value, cookie);

						return true;
					}
				}
			}

			return false;
		}

		private IEnumerable<MessageItem> InternalGetMessageList(string url)
		{
			var htmlContent = RequestHelper.Get(url, _loginContext.LoginCookie);

			if (!string.IsNullOrWhiteSpace(htmlContent))
			{
				var regex = new Regex(@"(?<=\({""msg_item"":\[).*(?=\]}\).msg_item)", RegexOptions.IgnoreCase);
				var content = regex.Match(htmlContent).Value;

				if (!string.IsNullOrWhiteSpace(content))
					return JsonHelper.Deserialize<List<MessageItem>>(string.Format("[{0}]", content));
			}

			return null;
		}

		#endregion
	}
}
