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
			var result = MPManager.GetAllMessageListAsync(20, 7).Result;
			//var result = MPManager.GetSingleSendMessageList("126185600").Result;

			foreach (var item in result)
			{
				Console.WriteLine("fakeId: {0}, nick name: {1}, content: {2}", item.fakeid, item.nick_name, item.content);
			}
			//Console.WriteLine(result);

			Console.ReadKey(true);
		}
	}
}
