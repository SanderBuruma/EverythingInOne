using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;
using PersonalWebsite.Models;

namespace PersonalWebsite.Controllers
{
   public class ScriboAlacritoController : BaseController
   {
      private static string[] _lines;
      private static Random _rng;
      
      private readonly IHttpContextAccessor _httpContextAccessor;

      public ScriboAlacritoController(IHttpContextAccessor httpContextAccessor)
      {
         _httpContextAccessor = httpContextAccessor;
      }

      public static void Initialize()
      {
         List<string> text = DataFiles.MeumProelium.Text.Split("\n").ToList();
         List<string> linesNew = new List<string>();
         Regex rgx = new Regex(@"(.{90,180}?.*?[.;,?!]\s+)|(.{150}.*?\s+)", RegexOptions.None);
         string leftOverLine = "";

         for (int i = 0; i < text.Count; i++)
         {
            text[i] = leftOverLine + " " + text[i];
            leftOverLine = "";

            if (text[i].Length > 150 && text[i].Length < 200)
            {
               linesNew.Add(text[i].Trim());
               continue;
            }
            else if (text[i].Length <= 150)
            {
               leftOverLine = text[i].Trim();
            }
            else
            {
               while (text[i].Length >= 200)
               {
                  var match = rgx.Match(text[i]).Value;
                  linesNew.Add(match.Trim());
                  text[i] = text[i].Substring(match.Length);
               }
               leftOverLine = text[i].Trim();
            }
         }
         linesNew.Add(leftOverLine.Trim());

         _lines = linesNew.ToArray();
         _rng = new Random();
      }
      
      [HttpGet("getText")]
      public object GetText(int i)
      {
         var context = _httpContextAccessor.HttpContext;
         
         string ip = context.Connection.RemoteIpAddress.ToString();
         var currentDate = DateTime.Now.ToUniversalTime();
         if (i < 0)
             i = 0;
         i %= _lines.Length;
         UserReports.Add(ip, i, currentDate);

         return new { str = _lines[i], i };
      }

      [HttpGet("getIps")]
      public object GetIps(string password)
      {
         // Step 1, calculate MD5 hash from password
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
         if ("27F2E18CCF46D8B3502BECE030B52BDB" == str)//A tiny little MD5 hash which should probably be removed...
         {
            //make the json string ourselves
            StringBuilder bldr = new StringBuilder("{");
            for (int i = 0; i < UserReports.Instances.Count; i++)
            {
               if (i>0) bldr.Append(",");
               bldr.Append('"');
               bldr.Append($"{UserReports.Instances[i].Ip}: [");
               bldr.Append('"');

               var list = new List<int>();
               UserReports.Instances[i].Events.ForEach(s=>list.Add(s.TextIndex));
               bldr.Append(list[0]);
               list.RemoveAt(0);
               foreach (var item in list)
               {
                  bldr.Append(String.Format(",{0}", item));
               }

               bldr.Append("]");
            }
            bldr.Append("}");

            return new { ips = bldr.ToString() };
         }

         return new { wrongPassword = true, tryAgain = false };
      }

      [HttpGet("getLinesNr")]
      public int GetLinesNr()
      {
         return _lines.Length;
      }
   }
}
