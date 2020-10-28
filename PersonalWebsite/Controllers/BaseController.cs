using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

		public string GetCookieById(string key){
         string returnString;
         _httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(key, out returnString);
         if (String.IsNullOrWhiteSpace(returnString)){
            return "";
         }

         return returnString;
      }

   }
}
