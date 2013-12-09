
namespace WX.Tools.DTOs
{
	public class MessageItem
	{
		public int id { get; set; }
		public int type { get; set; }
		public string fakeid { get; set; }
		public string nick_name { get; set; }
		public int date_time { get; set; }
		public string content { get; set; }
		public string source { get; set; }
		public int msg_status { get; set; }
		public int has_reply { get; set; }
		public string refuse_reason { get; set; }
	}
}
