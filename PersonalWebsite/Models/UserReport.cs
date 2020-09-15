using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalWebsite.Models
{
   public class UserReport
   {
      public UserReport(string internetProtocol, int textIndex, IHeaderDictionary hederDictionary)
      {
         InternetProtocol = internetProtocol;
         TextIndex = textIndex;
         HederDictionary = hederDictionary;
      }
      public string InternetProtocol { get; set; }
      public int TextIndex { get; set; }
      public IHeaderDictionary HederDictionary { get; set; }
   }
}
