
namespace MPHelper.DTOs
{
	/// <summary>
	/// 联系人对象
	/// </summary>
	public class ContactInfo
	{
		/// <summary>
		/// FakeId
		/// </summary>
		public long fake_id { get; set; }
		
		/// <summary>
		/// 昵称
		/// </summary>
		public string nick_name { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string user_name { get; set; }

		/// <summary>
		/// 个人签名
		/// </summary>
		public string signature { get; set; }

		/// <summary>
		/// 城市
		/// </summary>
		public string city { get; set; }

		/// <summary>
		/// 省
		/// </summary>
		public string province { get; set; }

		/// <summary>
		/// 国家
		/// </summary>
		public string country { get; set; }

		/// <summary>
		/// 性别（1：男； 2：女）
		/// </summary>
		public int gender { get; set; }

		/// <summary>
		/// 备注名
		/// </summary>
		public string remark_name { get; set; }

		/// <summary>
		/// 分组Id
		/// </summary>
		public int group_id { get; set; }
	}
}
