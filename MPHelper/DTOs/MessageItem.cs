using Newtonsoft.Json;

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
		[JsonProperty("id")]
		public int Id { get; set; }

		/// <summary>
		/// 消息类型
		/// </summary>
		[JsonProperty("type")]
		public int Type { get; set; }

		/// <summary>
		/// FakeId
		/// </summary>
		[JsonProperty("fakeid")]
		public string FakeId { get; set; }

		/// <summary>
		/// 昵称
		/// </summary>
		[JsonProperty("nick_name")]
		public string NickName { get; set; }

		/// <summary>
		/// 发送时间
		/// </summary>
		[JsonProperty("date_time")]
		public int DateTime { get; set; }

		/// <summary>
		/// 内容
		/// </summary>
		[JsonProperty("content")]
		public string Content { get; set; }

		/// <summary>
		/// 来源
		/// </summary>
		[JsonProperty("source")]
		public string Source { get; set; }

		/// <summary>
		/// 消息状态
		/// </summary>
		[JsonProperty("msg_status")]
		public int MsgStatus { get; set; }

		/// <summary>
		/// 是否回复
		/// </summary>
		[JsonProperty("has_reply")]
		public int HasReply { get; set; }

		/// <summary>
		/// 拒绝原因
		/// </summary>
		[JsonProperty("refuse_reason")]
		public string RefuseReason { get; set; }
	}
}
