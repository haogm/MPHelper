namespace MPHelper
{
	internal static class MPAddresses
	{
		/// <summary>
		/// 登录
		/// </summary>
		public const string LoginUrl = "https://mp.weixin.qq.com/cgi-bin/login?lang=zh_CN";

		/// <summary>
		/// 更改用户分组
		/// </summary>
		public const string ModifyCategoryUrl = "https://mp.weixin.qq.com/cgi-bin/modifycontacts";

		/// <summary>
		/// 单用户消息发送
		/// </summary>
		public const string SingleSendMessageUrl = "https://mp.weixin.qq.com/cgi-bin/singlesend";

		/// <summary>
		/// 群组消息推送
		/// </summary>
		public const string MassSendMessageUrl = "https://mp.weixin.qq.com/cgi-bin/masssend";

		/// <summary>
		/// 用户信息
		/// </summary>
		public const string GetContactinfoUrl = "https://mp.weixin.qq.com/cgi-bin/getcontactinfo";

		/// <summary>
		/// 所有用户消息列表
		/// </summary>
		public const string AllMessageListUrlFormat = "https://mp.weixin.qq.com/cgi-bin/message?t=message/list&count={0}&day={1}&token={2}&lang=zh_CN";

		/// <summary>
		/// 消息列表（关键字检索）
		/// </summary>
		public const string KeywordMessageListUrlFormat = "https://mp.weixin.qq.com/cgi-bin/message?t=message/list&action=search&keyword={0}&count={1}&token={2}&lang=zh_CN";

		/// <summary>
		/// 单个用户对话消息列表
		/// </summary>
		public const string SingleSendMessageListUrlFormat = "https://mp.weixin.qq.com/cgi-bin/singlesendpage?t=message/send&action=index&tofakeid={0}&token={1}&lang=zh_CN";

		/// <summary>
		/// 星标消息列表
		/// </summary>
		public const string StarMessageListUrlFormat = "https://mp.weixin.qq.com/cgi-bin/message?t=message/list&count={0}&action=star&token={1}&lang=zh_CN";

		/// <summary>
		/// 设置星标消息
		/// </summary>
		public const string SetStartMessageUrl = "https://mp.weixin.qq.com/cgi-bin/setstarmessage";

		/// <summary>
		/// 文件下载
		/// </summary>
		public const string DownloadFileUrlFormat = "https://mp.weixin.qq.com/cgi-bin/downloadfile?msgid={0}&source=&token={1}";
	}
}
