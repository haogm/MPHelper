using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WX.Tools.Test
{
	using Hanger.Common;
	using NUnit.Framework;

    public class BaseMethodTest
    {
		const string FAKE_ID = "126185600";
		const string CATEGORY_ID = "0";

		[Test]
		public void GetContactInfoTest()
		{
			var contactInfo = MPManager.GetContactInfoAsync(FAKE_ID).Result;

			if (contactInfo != null)
			{
				Console.WriteLine("ContactInfo: {0}", contactInfo.ObjectToJson());
			}

			Assert.NotNull(contactInfo);
			Assert.AreEqual(FAKE_ID, contactInfo.fake_id.ToString());
		}

		[Test]
		public void GetSingleSendMessageListTest()
		{
			var messages = MPManager.GetSingleSendMessageListAsync(FAKE_ID).Result;

			if (messages != null && messages.Count() > 0)
			{
				Console.WriteLine("FakeId: {0}", messages.First().fakeid);
			}

			Assert.NotNull(messages);
			Assert.IsTrue(messages.Count() > 0);
		}

		[Test]
		public void GetAllMessageListTest()
		{
			var messages = MPManager.GetAllMessageListAsync(1).Result;

			if (messages != null && messages.Count() > 0)
			{
				Console.WriteLine("FakeId: {0}", messages.First().fakeid);
			}

			Assert.NotNull(messages);
			Assert.IsTrue(messages.Count() > 0);
		}

		[Test]
		public void GetStarMessageListTest()
		{
			var messages = MPManager.GetStarMessageListAsync(1).Result;

			if (messages != null && messages.Count() > 0)
			{
				Console.WriteLine("FakeId: {0}", messages.First().fakeid);
			}

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
		public void SendMessageTest()
		{
			/*
			 * 可先给公众账号发送一条消息，确保突破24小时限制
			 */

			var message = "Hello world!";
			var success = MPManager.SendMessageAsync(FAKE_ID, message).Result;

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
