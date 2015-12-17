namespace MPHelper.Dtos
{
	/// <summary>
	/// 单条消息对象
	/// </summary>
	public class MessageItem
	{
		/// <summary>
		/// 消息ID
		/// </summary>
		public int id { get; set; }

		/// <summary>
		/// 消息类型
		/// </summary>
		public int type { get; set; }

		/// <summary>
		/// FakeId
		/// </summary>
		public string fakeid { get; set; }

		/// <summary>
		/// 昵称
		/// </summary>
		public string nick_name { get; set; }

		/// <summary>
		/// 发送时间
		/// </summary>
		public int date_time { get; set; }

		/// <summary>
		/// 内容
		/// </summary>
		public string content { get; set; }

		/// <summary>
		/// 来源
		/// </summary>
		public string source { get; set; }

		/// <summary>
		/// 消息状态
		/// </summary>
		public int msg_status { get; set; }

		/// <summary>
		/// 是否回复
		/// </summary>
		public int has_reply { get; set; }

		/// <summary>
		/// 拒绝原因
		/// </summary>
		public string refuse_reason { get; set; }
	}
}
