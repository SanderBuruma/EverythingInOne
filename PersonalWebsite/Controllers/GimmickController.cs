using Microsoft.AspNetCore.Mvc;
using Cubit32.Primes;
using System.Threading;
using Microsoft.AspNetCore.Http;

namespace PersonalWebsite.Controllers
{
   public class GimmickController : BaseController
   {

		public GimmickController(IHttpContextAccessor httpContextAccessor): base(httpContextAccessor)
		{
		}

		[HttpGet("bigPrime")]
      public object BigPrime(int digits, CancellationToken cToken)
      {
         return new { prime = Primes.Generate(digits, cToken).ToString("N0") };
      }
   }
}
