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
      private static List<BookIndices> _bookIndices = new List<BookIndices>();
      
      private readonly IHttpContextAccessor _httpContextAccessor;

      public ScriboAlacritoController(IHttpContextAccessor httpContextAccessor): base(httpContextAccessor)
      {
         _httpContextAccessor = httpContextAccessor;
      }

      public static void Initialize()
      {
         List<string> baseLines = new List<string>();

         InitializeText(DataFiles.HumanumGenus
            .Text.Split("\n").ToList(), baseLines, "Humanum Genus, Leo XIII");
         InitializeText(DataFiles.MoneyManipulationSocialOrder
            .Text.Split("\n").ToList(), baseLines, "Money Manipulation and the Social Order, Fr Fahey");
         InitializeText(DataFiles.MeumProelium
            .Text.Split("\n").ToList(), baseLines, "Meum Proelium, Artifex Germanus");

         _lines = baseLines.ToArray();
      }

      private static void InitializeText(List<string> text, List<string> baseLines, string title) {
         _bookIndices.Add(new BookIndices(baseLines.Count, title));
         List<string> linesNew = new List<string>();
         Regex rgx = new Regex(@"(.{90,180}?.*?[.;,?!]\s+)|(.{150}.*?\s+)", RegexOptions.None);
         string leftOverLine = "";

         for (int i = 0; i < text.Count; i++)
         {
            text[i] = leftOverLine + " " + text[i];
            leftOverLine = "";

            if (text[i].Length > 150 && text[i].Length < 200)
            {
               baseLines.Add(text[i].Trim());
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
                  baseLines.Add(match.Trim());
                  text[i] = text[i].Substring(match.Length);
               }
               leftOverLine = text[i].Trim();
            }
         }
         baseLines.Add(leftOverLine.Trim());
      }
      
      [HttpGet("getText")]
      public object GetText(int i)
      {
         HttpContext context = _httpContextAccessor.HttpContext;
         
         string ip = context.Connection.RemoteIpAddress.ToString();
         DateTime currentDate = DateTime.Now.ToUniversalTime();
         if (i < 0)
             i = 0;
         i %= _lines.Length;
         UserReports.Add(ip, i, currentDate);

         string title = "";
         foreach (BookIndices a in _bookIndices){
            if (a.i < i) {
               title = a.title;
            } else { break; }
         }

         return new { str = _lines[i], i, title };
      }

      [HttpGet("getIps")]
      public object GetIps(string password)
      {

         // Step 3, check if it matches the hash pw
         if (CheckSecretPhrase(password))//A tiny little MD5 hash which should probably be removed...
         {
            //make the json string ourselves
            StringBuilder bldr = new StringBuilder("{");
            for (int i = 0; i < UserReports.Instances.Count; i++)
            {
               if (i>0) bldr.Append(",");
               bldr.Append('"');
               bldr.Append($"{UserReports.Instances[i].Ip}");
               bldr.Append('"');
               bldr.Append(":[");

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

   public struct BookIndices {
		public BookIndices(int i, string title)
		{
			this.i = i;
			this.title = title;
		}
		public int i;
      public string title;
   }
}
