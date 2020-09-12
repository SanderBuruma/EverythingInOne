using Microsoft.AspNetCore.Mvc;
using Cubit32.Primes;
using System.Threading;

namespace PersonalWebsite.Controllers
{
   public class GimmickController : BaseController
   {

      [HttpGet("bigPrime")]
      public object BigPrime(int digits, CancellationToken cToken)
      {
         return new { prime = Primes.Generate(digits, cToken).ToString("N0") };
      }
   }
}
