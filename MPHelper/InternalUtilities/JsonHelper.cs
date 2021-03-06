﻿using Newtonsoft.Json;

namespace MPHelper.InternalUtilities
{
	internal static class JsonHelper
	{
		public static string Serialize(object obj)
		{
			return JsonConvert.SerializeObject(obj);
		}

		public static T Deserialize<T>(string json)
		{
			return string.IsNullOrWhiteSpace(json) ? default(T) : JsonConvert.DeserializeObject<T>(json);
		}
	}
}
