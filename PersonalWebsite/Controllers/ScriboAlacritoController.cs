using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PersonalWebsite.Controllers
{
   [ApiController]
   [Route("[controller]")]
   public class ScriboAlacritoController
   {
      public static List<string> _books;

      public void Initialize()
      {
         List<string> books = new List<string>();
         string meumProelium = "";
      }

      [HttpGet("getText")]
      public object GetText(CancellationToken cToken)
      {
         //return Environment.CurrentDirectory;

         //DirectoryInfo d = new DirectoryInfo(Environment.CurrentDirectory);//Assuming Test is your Folder
         //FileInfo[] Files = d.GetFiles("*.*"); //Getting Text files
         //string str = "";
         //foreach(FileInfo file in Files )
         //{
         //  str = str + ", " + file.Name;
         //}

         List<string> directories = new List<string>();
         
         Directory.GetDirectories(Environment.CurrentDirectory).ToList().ForEach(direc=>{
            if (!direc.StartsWith(".")) directories.Add(direc);
         });
         for (int i = 0; i < directories.Count && directories.Count < 10000; i++)
         {
            Directory.GetDirectories(directories[i]).ToList().ForEach(direc=>{
               if (!direc.StartsWith(".")) directories.Add(direc);
            });
         }


         List<string> str = new List<string>();
         foreach (var directory in directories)
         {
            DirectoryInfo d = new DirectoryInfo(directory);//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles("*.*"); //Getting Text files
            foreach (FileInfo file in Files)
            {
              str.Add(file.FullName);
            }
         }

         return new { str };

         string readContents;
         using (StreamReader streamReader = new StreamReader(@"C:\"))
         {
              readContents = streamReader.ReadToEnd();
         }
      }
   }
}
