using System;

namespace PersonalWebsite.Enumerables
{
	/// <summary>
	/// Pseudo enum. Used to get cookie values which match cookie values from ClientApp/Src/App/Shared/Enums/cookie-keys.enum.ts.
	/// </summary>
	public static class CookieKeys {
		public static string ScriboI { get { return "scribo-i"; } }
		public static string ScriboCharSum { get { return "scribo-sum-chars"; } }
		public static string ScriboTimeSum { get { return "scribo-sum-time"; } }

		public static string Count { get { return "count"; } }
		public static string ThemeIndex { get { return "theme-index"; } }

		public static string RngId { get { 
			return "rng-id"; 
		} }
	}
}
