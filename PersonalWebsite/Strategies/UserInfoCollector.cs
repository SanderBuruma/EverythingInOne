using Microsoft.AspNetCore.Http;
using PersonalWebsite.Enumerables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalWebsite.Strategies
{
	public static class UserInfoCollector
	{
		private static List<string> _messages = new List<string>();

		public static string[] Messages
		{
			get
			{
				return _messages.ToArray();
			}
		}

		public static void AddMessage(HttpContext context, string message) {
			string returnString;
			context.Request.Cookies.TryGetValue(CookieKeys.RngId, out returnString);
			if (String.IsNullOrWhiteSpace(returnString)){
				returnString = "";
			}

			StringBuilder bldr = new StringBuilder(String.Format("Guid:{0}; ", GetCookieById(CookieKeys.RngId, context)));

			bldr.Append(String.Format("msg:{0}; ", message));
			bldr.Append(String.Format("ip:{0}; ", context.Connection.RemoteIpAddress));

			foreach	(var i in context.Request.Headers) {
				bldr.Append(String.Format("{0}:{1}; ", i.Key, i.ToString()));
			}

			_messages.Add(bldr.ToString());
		}

		public static void ResetMessages() {
			_messages = new List<string>();
		}

		private static string GetCookieById(string key, HttpContext context){
			string returnString;
			context.Request.Cookies.TryGetValue(key, out returnString);
			if (String.IsNullOrWhiteSpace(returnString)){
				return "";
			}

			return returnString;
		}
	}
}