using System.Linq;

namespace MPHelper.Test
{
	using NUnit.Framework;
	using System.IO;

	public class BaseMethodTest
	{
		const string MP_ACCOUNT = "010227leo@gmail.com";
		const string MP_PASSWORD_MD5 = "498a5846ae15e26c96cffd8e21eb483b";
		const string FAKE_ID = "126185600";
		const string CATEGORY_ID = "0";

		private MPManager _MPManager;

		[SetUp]
		protected void TestSetUp()
		{
			_MPManager = new MPManager(MP_ACCOUNT, MP_PASSWORD_MD5);
		}

		[Test]
		public void GetContactInfoTest()
		{
			var contactInfo = _MPManager.GetContactInfoAsync(FAKE_ID).Result;

			Assert.NotNull(contactInfo);
			Assert.AreEqual(FAKE_ID, contactInfo.fake_id.ToString());
		}

		[Test]
		public void GetSingleSendMessageListTest()
		{
			var messages = _MPManager.GetSingleSendMessageListAsync(FAKE_ID).Result;

			Assert.NotNull(messages);
			Assert.IsTrue(messages.Count() > 0);
		}

		[Test]
		public void GetAllMessageListTest()
		{
			var messages = _MPManager.GetAllMessageListAsync(1).Result;

			Assert.NotNull(messages);
			Assert.IsTrue(messages.Count() > 0);
		}

		[Test]
		public void GetMessageListByKeywordTest()
		{
			var messages = _MPManager.GetMessageListByKeywordAsync("010227", 20).Result;

			Assert.NotNull(messages);
			Assert.IsTrue(messages.Count() > 0);
		}

		[Test]
		public void GetStarMessageListTest()
		{
			var messages = _MPManager.GetStarMessageListAsync(1).Result;

			Assert.NotNull(messages);
			Assert.IsTrue(messages.Count() > 0);
		}

		[Test]
		public void SetStarMessageTest()
		{
			var success = false;
			var messages = _MPManager.GetAllMessageListAsync(1).Result;

			if (messages != null && messages.Count() > 0)
			{
				success = _MPManager.SetStarMessageAsync(messages.First().id.ToString(), true).Result;
			}

			Assert.IsTrue(success);
		}

		[Test]
		public void SingleSendMessageTest()
		{
			/*
			 * 可先给公众账号发送一条消息，确保突破48小时限制。
			 */

			var success = true;

			var message = "SingleSendMessageTest: test from MPHelper! 中文消息测试！";
			success = _MPManager.SingleSendMessageAsync(FAKE_ID, MPMessageType.Text, message).Result;

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
			var success = _MPManager.ChangeCategoryAsync(FAKE_ID, CATEGORY_ID).Result;

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
