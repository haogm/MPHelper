namespace MPHelper.Test
{
	using NUnit.Framework;

	public class MultipleAccountsTest
	{
		const string MpAAccount = "010227leo@gmail.com";
		const string MpAPasswordMd5 = "498a5846ae15e26c96cffd8e21eb483b";
		const string MpBAccount = "yinwangbd@sina.com";
		const string MpBPasswordMd5 = "53e3ffe2aba20ce80453e3fe3b824928";
		const string FakeId = "126185600";

		private MpManager _mpAManager;
		private MpManager _mpBManager;

		[SetUp]
		protected void TestSetUp()
		{
			_mpAManager = new MpManager(MpAAccount, MpAPasswordMd5);
			_mpBManager = new MpManager(MpBAccount, MpBPasswordMd5);
		}


		[Test]
		public void SingleSendMessageTest()
		{
			const string message = "MultipleAccountsTest.SingleSendMessageTest: ok！";
			var successA = _mpAManager.SingleSendMessageAsync(FakeId, MpMessageType.Text, message).Result;
			var successB = _mpBManager.SingleSendMessageAsync(FakeId, MpMessageType.Text, message).Result;

			Assert.IsTrue(successA);
			Assert.IsTrue(successB);
		}
	}
}
