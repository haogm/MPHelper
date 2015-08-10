using System;
using System.IO;
using NUnit.Framework;

namespace MPHelper.Test
{
	public class Spider
	{
		const string MpAccount = "oerlikonwx@163.com";
		const string MpPasswordMd5 = "0AF92CED2C0DE88E9780106D2AB4F71B";

		private MpManager _mpManager;

		[SetUp]
		protected void TestSetUp()
		{
			_mpManager = new MpManager(MpAccount, MpPasswordMd5);
		}

		[Test]
		public void GetStatisticsTest()
		{
			var from = new DateTime(2014, 1, 1);
			var to = new DateTime(2015, 9, 1);

			using (var streamWriter = new StreamWriter("1.txt", false))
			{
				while (from < to)
				{
					var page = 1;
					var hasMore = true;

					while (hasMore)
					{
						var result = _mpManager.GetStatisticsAsync("wx6b6907213f405fe3", page++, from, from.AddMonths(1)).Result;

						if (result != null)
						{
							foreach (var item in result.data)
							{
								streamWriter.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t",
									item.title, item.time, item.index[0], item.index[1], item.index[4], item.index[8]);
							}

							hasMore = result.hasMore;
						}
						else
						{
							break;
						}
					}

					from = from.AddMonths(1);
				}
			}
		}
	}
}
