using Newtonsoft.Json;

namespace MPHelper.Dtos
{
	/// <summary>
	/// 联系人对象
	/// </summary>
	public class ContactInfo
	{
		/// <summary>
		/// FakeId
		/// </summary>
		[JsonProperty("fake_id")]
		public string FakeId { get; set; }
		
		/// <summary>
		/// 昵称
		/// </summary>
		[JsonProperty("nick_name")]
		public string NickName { get; set; }

		/// <summary>
		/// 微信号
		/// </summary>
		[JsonProperty("user_name")]
		public string UserName { get; set; }

		/// <summary>
		/// 个人签名
		/// </summary>
		[JsonProperty("signature")]
		public string Signature { get; set; }

		/// <summary>
		/// 城市
		/// </summary>
		[JsonProperty("city")]
		public string City { get; set; }

		/// <summary>
		/// 省
		/// </summary>
		[JsonProperty("province")]
		public string Province { get; set; }

		/// <summary>
		/// 国家
		/// </summary>
		[JsonProperty("country")]
		public string Country { get; set; }

		/// <summary>
		/// 性别（1：男； 2：女）
		/// </summary>
		[JsonProperty("gender")]
		public int Gender { get; set; }

		/// <summary>
		/// 备注名
		/// </summary>
		[JsonProperty("remark_name")]
		public string RemarkName { get; set; }

		/// <summary>
		/// 分组Id
		/// </summary>
		[JsonProperty("group_id")]
		public int GroupId { get; set; }
	}
}
