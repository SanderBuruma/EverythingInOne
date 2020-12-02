using Nancy.Json;
using System;
using System.Net;
using AutoMapper;
using NuNlScraper.Classes;
using HtmlAgilityPack;
using System.Collections.Generic;

namespace NuNlScraper
{
   class Program
   {
      static void Main(string[] args)
      {
         var client = new WebClient();

         string jsonString = client.DownloadString(String.Format("https://www.nu.nl/block/lean_json/articlelist?limit={0}&offset=0&source=latest&filter=site", 20));
         JavaScriptSerializer json_serializer = new JavaScriptSerializer();
         NuData dc = json_serializer.Deserialize<NuData>(jsonString);

         var url = string.Format("https://nu.nl/{0}",
               dc.Data.Context.Articles.Find(s=>s.Type!="video").Url
         );

         string html = client.DownloadString(url);
         HtmlDocument doc = new HtmlDocument();
         doc.LoadHtml(html);

         var temp = new List<HtmlNode>();
         foreach (var item in doc.DocumentNode.Descendants("p"))
         {
            temp.Add(item);
         }
      }
   }
}
