using System.ComponentModel;

namespace MPHelper.Dtos
{
	/// <summary>
	/// 发送消息类型
	/// </summary>
	public enum MpMessageType
	{
		/// <summary>
		/// 文字
		/// </summary>
		[Description("文字")] Text = 1,

		/// <summary>
		/// 图片
		/// </summary>
		[Description("图片")] Image = 2,

		/// <summary>
		/// 音频
		/// </summary>
		[Description("音频")] Audio = 3,

		/// <summary>
		/// 视频
		/// </summary>
		[Description("视频")] Video = 15,

		/// <summary>
		/// 图文
		/// </summary>
		[Description("图文")] AppMsg = 10
	}
}
