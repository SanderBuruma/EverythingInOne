using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalWebsite.Models
{
   public static class UserReports
   {

      public static void Add(string ip, int textIndex, DateTime date)
      {
         int index = Instances.FindIndex(
            instance=>{
               return instance.Ip == ip;
            }
         );
         if (index == -1)
         {
            IpWEvents addMe2 = new IpWEvents(ip, textIndex, date);
            Instances.Add(addMe2);
         }
         else
         {
            Instances[index].Events.Add(new SomeEvent(textIndex, date));
         }
      }

      public static List<IpWEvents> Instances { get; set; } = new List<IpWEvents>();
   }

   public class IpWEvents
   {
      public IpWEvents(string ip, int textIndex, DateTime date)
      {
         Ip = ip;
         Events = new List<SomeEvent>();
         Events.Add(new SomeEvent(textIndex, date));
      }
      public string Ip;
      public List<SomeEvent> Events;
   }

   public class SomeEvent
   {
      public SomeEvent(int textIndex, DateTime date)
      {
         TextIndex = textIndex;
         Date = date;
      }
      public int TextIndex;
      public DateTime Date;
   }
}
