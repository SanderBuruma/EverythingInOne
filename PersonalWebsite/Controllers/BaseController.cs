using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PersonalWebsite.Controllers
{
   [ApiController]
   [Route("[controller]")]
   public class BaseController : Controller
   {
      private readonly IHttpContextAccessor _httpContextAccessor;
      protected Random _rng;

		public BaseController(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
         _rng = new Random();
		}

		protected string GetCookieById(string key){
         string returnString;
         _httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(key, out returnString);
         if (String.IsNullOrWhiteSpace(returnString)){
            return "";
         }

         return returnString;
      }

      protected static bool CheckSecretPhrase(string phrase) {

         // Step 1, calculate MD5 hash from password
         MD5 md5 = MD5.Create();
         byte[] inputBytes = Encoding.ASCII.GetBytes(phrase);
         byte[] hashBytes = md5.ComputeHash(inputBytes);
     
         // Step 2, convert byte array to hex string
         StringBuilder sb = new StringBuilder();
         for (int i = 0; i < hashBytes.Length; i++)
         {
            sb.Append(hashBytes[i].ToString("X2"));
         }
         string str = sb.ToString();

         // Step 3, check if it matches the hash pw
         return "27F2E18CCF46D8B3502BECE030B52BDB" == str; //A tiny little MD5 hash which should probably be removed...
      }

   }
}
