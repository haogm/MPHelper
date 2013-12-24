namespace MPHelper.Utility
{
	using Newtonsoft.Json;

	internal class JsonHelper
	{
		public static string Serialize(object obj)
		{
			return JsonConvert.SerializeObject(obj);
		}

		public static T Deserialize<T>(string json)
		{
			if (string.IsNullOrWhiteSpace(json))
				return default(T);

			try
			{
				return JsonConvert.DeserializeObject<T>(json);
			}
			catch
			{
				return default(T);
			}
		}
	}
}
