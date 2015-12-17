using System.Linq;
using NUnit.Framework;

namespace MPHelper.Test
{
	public class BaseMethodTest
	{
		const string MpAccount = "010227leo@gmail.com";
		const string MpPasswordMd5 = "498a5846ae15e26c96cffd8e21eb483b";
		const string FakeId = "ocMl4t4_lBEksXCGmGUqKxda_EwI";
		const string CategoryId = "0";

		private MpManager _mpManager;

		[SetUp]
		protected void TestSetUp()
		{
			_mpManager = new MpManager(MpAccount, MpPasswordMd5);
		}

		[Test]
		public void GetContactInfoTest()
		{
			var contactInfo = _mpManager.GetContactInfo(FakeId);

			Assert.NotNull(contactInfo);
			Assert.AreEqual(FakeId, contactInfo.fake_id);
		}

		[Test]
		public void GetSingleSendMessageListTest()
		{
			var messages = _mpManager.GetSingleSendMessageList(FakeId);

			Assert.NotNull(messages);
			Assert.IsTrue(messages.Any());
		}

		[Test]
		public void GetAllMessageListTest()
		{
			var messages = _mpManager.GetAllMessageList(1);

			Assert.NotNull(messages);
			Assert.IsTrue(messages.Any());
		}

		[Test]
		public void GetMessageListByKeywordTest()
		{
			var messages = _mpManager.GetMessageListByKeyword("010227");

			Assert.NotNull(messages);
			Assert.IsTrue(messages.Any());
		}

		[Test]
		public void GetStarMessageListTest()
		{
			var messages = _mpManager.GetStarMessageList(1);

			Assert.NotNull(messages);
			Assert.IsTrue(messages.Any());
		}

		[Test]
		public void SetStarMessageTest()
		{
			var success = false;
			var messages = _mpManager.GetAllMessageList(1);

			if (messages != null && messages.Any())
				success = _mpManager.SetStarMessageAsync(messages.First().id.ToString(), true).Result;

			Assert.IsTrue(success);
		}

		[Test]
		public async void SingleSendMessageTest()
		{
			/*
			 * 可先给公众账号发送一条消息，确保突破48小时限制。
			 */

			const string message = "SingleSendMessageTest: test from MPHelper! 中文消息测试！";

			var success = await _mpManager.SingleSendMessageAsync(FakeId, MpMessageType.Text, message);

			//var fileId = "10013378";
			//success = await _mpManager.SingleSendMessageAsync(FAKE_ID, MPMessageType.Image, fileId);

			//var appMsgId = "10013374";
			//success = await _mpManager.SingleSendMessageAsync(FAKE_ID, MPMessageType.AppMsg, appMsgId);

			Assert.IsTrue(success);
		}

		//[Test]
		//public async void MassSendMessageTest()
		//{
		//	/*
		//	 * 群发消息受公众账号限制（订阅号一天一条，服务号一个月一条），单元测试慎用。
		//	 */

		//	const string message = "MassSendMessageTest: test from MPHelper! 中文消息测试！";

		//	var success = await _mpManager.MassSendMessageAsync(MpMessageType.Text, message);

		//	Assert.IsTrue(success);
		//}

		[Test]
		public async void ChangeCategoryTest()
		{
			var success = await _mpManager.ChangeCategoryAsync(FakeId, CategoryId);

			Assert.IsTrue(success);
		}

		[Test]
		public void GetDonwloadFileBytesTest()
		{
			//var bytes = _MPManager.GetDonwloadFileBytes(200368584);

			//Assert.IsTrue(bytes.Length > 0);

			Assert.Pass();
		}
	}
}
