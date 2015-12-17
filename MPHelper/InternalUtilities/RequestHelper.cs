using System;
using System.IO;
using System.Net;
using System.Text;

namespace MPHelper.InternalUtilities
{
	internal static class RequestHelper
	{
		public static string Post(string url, string postData, CookieContainer cookie, Encoding encoding = null)
		{
#if DEBUG
			Console.WriteLine("Post:{0}", url);
#endif
			var byteArray = Encoding.UTF8.GetBytes(postData);
			var request = (HttpWebRequest) WebRequest.Create(url);

			request.Accept = "application/json, text/javascript, */*; q=0.01";
			request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
			request.ContentLength = byteArray.Length;
			request.CookieContainer = cookie;
			request.Headers.Add("Accept-Encoding", "gzip,deflate,sdch");
			request.Headers.Add("Accept-Language", "zh-cn");
			request.Headers.Add("Origin", "https://mp.weixin.qq.com");
			request.Headers.Add("X-Requested-With", "XMLHttpRequest");
			request.Host = "mp.weixin.qq.com";
			request.KeepAlive = true;
			request.Method = "POST";
			request.Referer = "https://mp.weixin.qq.com/";
			request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/30.0.1599.101 Safari/537.36";
			request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

			using (var requestStream = request.GetRequestStream())
			{
				requestStream.Write(byteArray, 0, byteArray.Length);
			}

			using (var response = (HttpWebResponse) request.GetResponse())
			{
				var responseStream = response.GetResponseStream();

				if (responseStream != null)
				{
					using (var streamReader = new StreamReader(responseStream, encoding ?? Encoding.UTF8))
					{
						return streamReader.ReadToEnd();
					}
				}
			}

			return string.Empty;
		}

		public static string Get(string url, CookieContainer cookie, string host = "mp.weixin.qq.com", Encoding encoding = null)
		{
#if DEBUG
			Console.WriteLine("Get:{0}", url);
#endif
			var request = (HttpWebRequest) WebRequest.Create(url);

			request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
			request.CookieContainer = cookie;
			request.ContentType = "application/x-www-form-urlencoded";
			request.Headers.Add("Accept-Encoding", "gzip,deflate,sdch");
			request.Headers.Add("Accept-Language", "zh-cn");
			request.Host = host;
			request.KeepAlive = true;
			request.Method = "GET";
			request.Referer = "https://mp.weixin.qq.com/";
			request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/30.0.1599.101 Safari/537.36";
			request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

			using (var response = (HttpWebResponse) request.GetResponse())
			{
				var responseStream = response.GetResponseStream();

				if (responseStream != null)
				{
					using (var streamReader = new StreamReader(responseStream, encoding ?? Encoding.UTF8))
					{
						return streamReader.ReadToEnd();
					}
				}
			}

			return string.Empty;
		}

		public static byte[] GetDonwloadFileBytes(string url, CookieContainer cookie)
		{
			var request = (HttpWebRequest) WebRequest.Create(url);

			request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
			request.CookieContainer = cookie;
			request.ContentType = "application/x-www-form-urlencoded";
			request.Headers.Add("Accept-Encoding", "gzip,deflate,sdch");
			request.Headers.Add("Accept-Language", "zh-cn");
			request.Host = "mp.weixin.qq.com";
			request.KeepAlive = true;
			request.Method = "GET";
			request.Referer = "https://mp.weixin.qq.com/";
			request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/30.0.1599.101 Safari/537.36";

			using (var response = (HttpWebResponse) request.GetResponse())
			{
				var buffer = new byte[response.ContentLength];

				var responseStream = response.GetResponseStream();

				if (responseStream != null)
				{
					responseStream.Read(buffer, 0, buffer.Length);

					return buffer;
				}
			}

			return null;
		}
	}
}
