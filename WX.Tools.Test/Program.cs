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
			var result = MPManager.GetLastedMessages().Result;

			//Console.WriteLine(result);

			Console.ReadKey(true);
		}
	}
}
