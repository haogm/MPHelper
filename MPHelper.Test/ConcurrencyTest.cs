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
				var mpManager = new MpManager(ConstData.MpAccount, ConstData.MpPasswordMd5).Preheat(true);
				var messages = mpManager.GetAllMessageList(3);

				Console.WriteLine("messages:{0}", messages.Count());
			});
		}
	}
}
