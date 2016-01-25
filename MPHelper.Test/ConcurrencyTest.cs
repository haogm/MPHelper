using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace MPHelper.Test
{
	public class ConcurrencyTest
	{
		[Test]
		public void PreheatTest()
		{
			Parallel.For(0, 10, _ =>
			{
				var mpManager = MpManager.GetInstance(ConstData.MpUserName, ConstData.MpPwdMd5).Preheat();
				var messages = mpManager.GetAllMessageList(3);

				Console.WriteLine("MpManager HashCode: {0}, Message count: {1}", mpManager.GetHashCode(), messages.Count());
			});
		}
	}
}
