using Microsoft.AspNetCore.Mvc;
using Cubit32.Primes;
using System.Threading;

namespace PersonalWebsite.Controllers
{
   [ApiController]
   [Route("[controller]")]
   public class GimmickController : ControllerBase
   {

      [HttpGet("bigPrime")]
      public object BigPrime(int digits, CancellationToken cToken)
      {
         return new { prime = Primes.Generate(digits, cToken).ToString("N0") };
      }
   }
}
