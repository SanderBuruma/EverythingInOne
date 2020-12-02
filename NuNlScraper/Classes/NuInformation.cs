using System.Collections.Generic;

namespace NuNlScraper.Classes
{
   class NuInformation
   {
      public int Limit { get; set; }
      public int Offset { get; set; }
      public string Source { get; set; }
      public string Filter { get; set; }
      public string Block_Id { get; set; }
      public List<NuArticle> Articles { get; set; }
      public string Section { get; set; }
      public bool Advertisement { get; set; }
      public string Template { get; set; }

   }
}
