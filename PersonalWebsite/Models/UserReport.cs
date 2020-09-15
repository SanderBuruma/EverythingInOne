using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalWebsite.Models
{
   public class UserReport
   {
      public UserReport(string internetProtocol, int textIndex, DateTime date)
      {
         InternetProtocol = internetProtocol;
         TextIndex = textIndex;
         Date = date;
      }
      public string InternetProtocol { get; set; }
      public int TextIndex { get; set; }
      public DateTime Date { get; set; }
   }
}
