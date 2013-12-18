using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WX.Tools.Test
{
	using Hanger.Common;
	using WX.Tools.Utility;

	class Program
	{
		static void Main(string[] args)
		{
			//var result = MPManager.ChangeCategoryAsync("126185600", "100").Result;
			//var result = MPManager.GetAllMessageListAsync(20, 7).Result;
			//var result = MPManager.GetSingleSendMessageListAsync("126185600").Result;
			//var result = MPManager.SendMessageAsync("126185600", "Hello world! 你好世界！").Result;

			var result = MPManager.GetContactInfoAsync("126185600").Result;

			if (result != null)
			{
				Console.WriteLine(result.ObjectToJson());
			}

			Console.ReadKey(true);
		}
	}
}
