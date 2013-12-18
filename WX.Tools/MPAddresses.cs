
namespace WX.Tools
{
	internal class MPAddresses
	{
		/// <summary>
		/// 登录
		/// </summary>
		public const string LOGIN_URL = "http://mp.weixin.qq.com/cgi-bin/login?lang=zh_CN";

		/// <summary>
		/// 更改用户分组
		/// </summary>
		public const string MODIFY_CATEGORY_URL = "https://mp.weixin.qq.com/cgi-bin/modifycontacts";

		/// <summary>
		/// 发送消息
		/// </summary>
		public const string SEND_MESSAGE_URL = "https://mp.weixin.qq.com/cgi-bin/singlesend";

		/// <summary>
		/// 用户信息
		/// </summary>
		public const string GET_USERINFO_URL = "https://mp.weixin.qq.com/cgi-bin/getcontactinfo";

		/// <summary>
		/// 所有用户消息列表
		/// </summary>
		public const string ALL_MESSAGE_LIST_URL_FORMAT = "https://mp.weixin.qq.com/cgi-bin/message?t=message/list&count={0}&day={1}&token={2}&lang=zh_CN";

		/// <summary>
		/// 单个用户对话消息列表
		/// </summary>
		public const string SINGLE_SEND_MESSAGE_LIST_URL_FORMAT = "https://mp.weixin.qq.com/cgi-bin/singlesendpage?t=message/send&action=index&tofakeid={0}&token={1}&lang=zh_CN";
	}
}
