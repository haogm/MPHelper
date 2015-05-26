using System.Linq;
using NUnit.Framework;

namespace MPHelper.Test
{
	public class BaseMethodTest
	{
		const string MpAccount = "010227leo@gmail.com";
		const string MpPasswordMd5 = "498a5846ae15e26c96cffd8e21eb483b";
		const string FakeId = "126185600";
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
			var contactInfo = _mpManager.GetContactInfoAsync(FakeId).Result;

			Assert.NotNull(contactInfo);
			Assert.AreEqual(FakeId, contactInfo.fake_id.ToString());
		}

		[Test]
		public void GetSingleSendMessageListTest()
		{
			var messages = _mpManager.GetSingleSendMessageListAsync(FakeId).Result;

			Assert.NotNull(messages);
			Assert.IsTrue(messages.Any());
		}

		[Test]
		public void GetAllMessageListTest()
		{
			var messages = _mpManager.GetAllMessageListAsync(1).Result;

			Assert.NotNull(messages);
			Assert.IsTrue(messages.Any());
		}

		[Test]
		public void GetMessageListByKeywordTest()
		{
			var messages = _mpManager.GetMessageListByKeywordAsync("010227").Result;

			Assert.NotNull(messages);
			Assert.IsTrue(messages.Any());
		}

		[Test]
		public void GetStarMessageListTest()
		{
			var messages = _mpManager.GetStarMessageListAsync(1).Result;

			Assert.NotNull(messages);
			Assert.IsTrue(messages.Any());
		}

		[Test]
		public void SetStarMessageTest()
		{
			var success = false;
			var messages = _mpManager.GetAllMessageListAsync(1).Result;

			if (messages != null && messages.Any())
				success = _mpManager.SetStarMessageAsync(messages.First().id.ToString(), true).Result;

			Assert.IsTrue(success);
		}

		[Test]
		public void SingleSendMessageTest()
		{
			/*
			 * 可先给公众账号发送一条消息，确保突破48小时限制。
			 */

			const string message = "SingleSendMessageTest: test from MPHelper! 中文消息测试！";
			var success = _mpManager.SingleSendMessageAsync(FakeId, MpMessageType.Text, message).Result;

			//var fileId = "10013378";
			//success = MPManager.SingleSendMessageAsync(FAKE_ID, MPMessageType.Image, fileId).Result;

			//var appMsgId = "10013374";
			//success = MPManager.SingleSendMessageAsync(FAKE_ID, MPMessageType.AppMsg, appMsgId).Result;

			Assert.IsTrue(success);
		}

		[Test]
		public void MassSendMessageTest()
		{
			/*
			 * 群发消息受公众账号限制（订阅号一天一条，服务号一个月一条），单元测试慎用。
			 */

			//var message = "MassSendMessageTest: test from MPHelper! 中文消息测试！";

			//var success = _MPManager.MassSendMessageAsync(MPMessageType.Text, message).Result;

			//Assert.IsTrue(success);

			Assert.Pass();
		}

		[Test]
		public void ChangeCategoryTest()
		{
			var success = _mpManager.ChangeCategoryAsync(FakeId, CategoryId).Result;

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
