using System;

namespace NuNlScraper.Classes
{
   class NuArticle
   {
      public int Id { get; set; }
      public string Slug { get; set; }
      public string Type { get; set; }
      public string Title { get; set; }
      public string Subtitle { get; set; }
      public DateTime Published_At { get; set; }
      public DateTime Updated_at { get; set; }
      public string Media_Id { get; set; }
      //public int duration { get; set; }
      public string Url { get; set; }
      public bool Open_New_Window { get; set; }
      //public string partner { get; set; }
      public CanonicalSection Canonical_Section { get; set; }
   }

   class CanonicalSection
   {
      public int Id { get; set; }
      public string Slug { get; set; }
      public string Type { get; set; }
      public string Name { get; set; }
      public string Style { get; set; }
      //public string media_id { get; set; }
      //public string partner { get; set; }
   }
}
