using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SnakeGame;

namespace PersonalWebsite.Controllers
{
   public class SnakeController : BaseController
   {
      public static void Initialize()
      {
         
      }

      [HttpGet("board")]
      public object ReturnBoard(int widthHeight, int i)
      {
         var board = new Board(widthHeight, i);
         return new { board };
      }
   }
}
