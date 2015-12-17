using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MPHelper.Dtos;
using NUnit.Framework;

namespace MPHelper.Test
{
	public class Spider
	{
		private MpManager _mpManager;

		[SetUp]
		protected void TestSetUp()
		{
			_mpManager = new MpManager(ConstData.MpAccount, ConstData.MpPasswordMd5).Preheat(true);
		}

		[Test]
		public void GetStatisticsTest()
		{
			var from = new DateTime(2013, 11, 1);
			var to = new DateTime(2014, 2, 1);

			var resultList = new List<StatisticsItem>();
			var actions = new List<Action>();

			while (from < to)
			{
				var begin = from;
				var end = from.AddMonths(1).AddDays(-1);

				actions.Add(() => resultList.AddRange(GetStatistics(begin, end)));

				from = from.AddMonths(1);
			}

			Task.Factory.StartNew(() => Parallel.Invoke(actions.ToArray())).Wait();

			using (var streamWriter = new StreamWriter("1.txt", false))
			{
				foreach (var item in resultList.OrderBy(i => i.time))
					streamWriter.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t",
						item.title, item.time, item.index[0], item.index[1], item.index[4], item.index[8]);
			}
		}

		private IEnumerable<StatisticsItem> GetStatistics(DateTime begin, DateTime end)
		{
			var resultList = new List<StatisticsItem>();
			var page = 1;
			var hasMore = true;

			while (hasMore)
			{
				var result = _mpManager.GetStatistics(ConstData.MpId, page++, begin, end);

				if (result == null)
					break;

				resultList.AddRange(result.data);

				hasMore = result.hasMore;
			}

			return resultList;
		}
	}
}
