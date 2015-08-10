using System.Collections.Generic;

namespace MPHelper.DTOs
{
	public class StatisticsInfo
	{
		public bool hasMore { get; set; }

		public List<StatisticsItem> data { get; set; }
	}

	public class StatisticsItem
	{
		public string id { get; set; }

		public string title { get; set; }

		public string time { get; set; }

		public List<string> index { get; set; }
	}
}
