using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;

namespace PersonalWebsite.Controllers
{
   public class ScriboAlacritoController : BaseController
   {
      private static string[] _lines;
      private static Random _rng;
      private static HashSet<string> _ipsRequested = new HashSet<string>();
      
      private readonly IHttpContextAccessor _httpContextAccessor;

       public ScriboAlacritoController(IHttpContextAccessor httpContextAccessor)
       {
           _httpContextAccessor = httpContextAccessor;
       }

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
         var context = _httpContextAccessor.HttpContext;

         string ip = context.Connection.RemoteIpAddress.ToString();
         _ipsRequested.Add(ip);

         if (i < 0)
             i = _rng.Next(_lines.Length);

         return new { str = _lines[i%_lines.Length], i };
      }

      [HttpGet("getIps")]
      public object GetIps(string password)
      {
         // Step 1, calculate MD5 hash from input
         MD5 md5 = MD5.Create();
         byte[] inputBytes = Encoding.ASCII.GetBytes(password);
         byte[] hashBytes = md5.ComputeHash(inputBytes);
     
         // Step 2, convert byte array to hex string
         StringBuilder sb = new StringBuilder();
         for (int i = 0; i < hashBytes.Length; i++)
         {
            sb.Append(hashBytes[i].ToString("X2"));
         }
         var str = sb.ToString();

         // Step 3, check if it matches the hash pw
         if ("27F2E18CCF46D8B3502BECE030B52BDB" == str)//A tiny little MD5 hash for... something...
         {
            return new { ips = _ipsRequested };
         }

         var empty = new HashSet<string>();
         empty.Add("UNAUTHORIZED: invalid password");
         return new { ips = empty };
      }
   }
}
