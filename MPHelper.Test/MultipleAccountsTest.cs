
namespace MPHelper.Test
{
	using NUnit.Framework;

	public class MultipleAccountsTest
	{
		const string MP_A_ACCOUNT = "010227leo@gmail.com";
		const string MP_A_PASSWORD_MD5 = "498a5846ae15e26c96cffd8e21eb483b";
		const string MP_B_ACCOUNT = "yinwangbd@sina.com";
		const string MP_B_PASSWORD_MD5 = "53e3ffe2aba20ce80453e3fe3b824928";
		const string FAKE_ID = "126185600";

		private MPManager _MP_A_Manager;
		private MPManager _MP_B_Manager;

		[SetUp]
		protected void TestSetUp()
		{
			_MP_A_Manager = new MPManager(MP_A_ACCOUNT, MP_A_PASSWORD_MD5);
			_MP_B_Manager = new MPManager(MP_B_ACCOUNT, MP_B_PASSWORD_MD5);
		}


		[Test]
		public void SingleSendMessageTest()
		{
			var message = "MultipleAccountsTest.SingleSendMessageTest: ok！";
			var success_A = _MP_A_Manager.SingleSendMessageAsync(FAKE_ID, MPMessageType.Text, message).Result;
			var success_B = _MP_B_Manager.SingleSendMessageAsync(FAKE_ID, MPMessageType.Text, message).Result;

			Assert.IsTrue(success_A);
			Assert.IsTrue(success_B);
		}
	}
}
