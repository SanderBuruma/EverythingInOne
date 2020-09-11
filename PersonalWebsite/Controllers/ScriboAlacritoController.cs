using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace PersonalWebsite.Controllers
{
   [ApiController]
   [Route("[controller]")]
   public class ScriboAlacritoController
   {
      private static string[] _lines;
      private static Random _rng;

      public static void Initialize()
      {
         List<string> text = DataFiles.MeumProelium.Text.Split("\n").ToList();
         List<string> linesNew = new List<string>();
         Regex rgx = new Regex(@".{150}.*?\s+", RegexOptions.None);
         foreach (string line in text)
         {
            if (line.Length < 300) linesNew.Add(line.Trim());
            else
            {
               string line2 = line;
               while (line2.Length > 150)
               {
                  string match = rgx.Match(line2).Value;
                  line2 = line2.Substring(match.Length);
                  if (match.Length < 15) break;
                  linesNew.Add(match.Trim());
               }
               if (line2.Length > 15)
                  linesNew.Add(line2.Trim());
            }
         }
         _lines = linesNew.ToArray();
         _rng = new Random();
      }

      [HttpGet("getText")]
      public object GetText(int i)
      {
         if (i < 0)
             i = _rng.Next(_lines.Length);

         return new { str = _lines[i%_lines.Length], i };
      }
   }
}
