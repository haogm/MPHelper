﻿using System.Linq;

namespace MPHelper.Test
{
	using NUnit.Framework;

    public class BaseMethodTest
    {
		const string FAKE_ID = "126185600";
		const string CATEGORY_ID = "0";

		[Test]
		public void GetContactInfoTest()
		{
			var contactInfo = MPManager.GetContactInfoAsync(FAKE_ID).Result;

			Assert.NotNull(contactInfo);
			Assert.AreEqual(FAKE_ID, contactInfo.fake_id.ToString());
		}

		[Test]
		public void GetSingleSendMessageListTest()
		{
			var messages = MPManager.GetSingleSendMessageListAsync(FAKE_ID).Result;

			Assert.NotNull(messages);
			Assert.IsTrue(messages.Count() > 0);
		}

		[Test]
		public void GetAllMessageListTest()
		{
			var messages = MPManager.GetAllMessageListAsync(1).Result;

			Assert.NotNull(messages);
			Assert.IsTrue(messages.Count() > 0);
		}

		[Test]
		public void GetStarMessageListTest()
		{
			var messages = MPManager.GetStarMessageListAsync(1).Result;

			Assert.NotNull(messages);
			Assert.IsTrue(messages.Count() > 0);
		}

		[Test]
		public void SetStarMessageTest()
		{
			var success = false;
			var messages = MPManager.GetAllMessageListAsync(1).Result;

			if (messages != null && messages.Count() > 0)
			{
				success = MPManager.SetStarMessageAsync(messages.First().id.ToString(), true).Result;
			}

			Assert.IsTrue(success);
		}

		[Test]
		public void SingleSendMessageTest()
		{
			/*
			 * 可先给公众账号发送一条消息，确保突破24小时限制
			 */

			var message = "SingleSendMessageTest: test from MPHelper! 中文消息测试！";
			var success = MPManager.SingleSendMessageAsync(FAKE_ID, MPMessageType.Text, message).Result;

			//var fileId = "10013378";
			//var success = MPManager.SingleSendMessageAsync(FAKE_ID, MPMessageType.Image, fileId).Result;

			//var appMsgId = "10013374";
			//var success = MPManager.SingleSendMessageAsync(FAKE_ID, MPMessageType.AppMsg, appMsgId).Result;

			Assert.IsTrue(success);
		}

		[Test]
		public void MassSendMessageTest()
		{
			var message = "MassSendMessageTest: test from MPHelper! 中文消息测试！";
			var success = MPManager.MassSendMessageAsync(MPMessageType.Text, message).Result;

			Assert.IsTrue(success);
		}

		[Test]
		public void ChangeCategoryTest()
		{
			var success = MPManager.ChangeCategoryAsync(FAKE_ID, CATEGORY_ID).Result;

			Assert.IsTrue(success);
		}
    }
}
