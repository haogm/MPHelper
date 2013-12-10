using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WX.Tools.Test
{
	using WX.Tools.Utility;

	class Program
	{
		static void Main(string[] args)
		{
			//var result = MPManager.ChangeCategoryAsync("760344340", "100").Result;
			//var result = MPManager.GetAllMessageList(20, 7).Result;
			var result = MPManager.GetSingleSendMessageList("126185600").Result;

			foreach (var item in result)
			{
				Console.WriteLine(item.content);
			}
			//Console.WriteLine(result);

			Console.ReadKey(true);
		}
	}
}
