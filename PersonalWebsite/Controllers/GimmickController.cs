using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Cubit32.Primes;

namespace PersonalWebsite.Controllers
{
   [ApiController]
   [Route("[controller]")]
   public class GimmickController : ControllerBase
   {

      [HttpGet("bigPrime")]
      public object BigPrime(int digits)
      {
         return new { prime = Primes.Generate(digits).ToString("N0") };
      }
   }
}
