using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cubit32.ColoredConsole
{
   class ColoredConsole
   {
      public ConsoleColor StandardColor { get; set; }
      public ConsoleColor OutputColor { get; set; }
      public ConsoleColor WarningColor { get; set; }
      public ConsoleColor InputColor { get; set; }

      public ColoredConsole(ConsoleColor standardColor = ConsoleColor.White, ConsoleColor outputColor = ConsoleColor.Green, ConsoleColor warningColor = ConsoleColor.Magenta, ConsoleColor inputColor = ConsoleColor.Red)
      {
         StandardColor = standardColor;
         OutputColor = outputColor;
         WarningColor = warningColor;
         InputColor = inputColor;
      }

      /// <summary>
      /// Gets String input from the user
      /// </summary>
      /// <param name="msg">The message to prompt the user with</param>
      /// <returns></returns>
      String GetConsoleInput(String msg = "")
      {
         Console.ForegroundColor = StandardColor;
         Console.Write(msg);
         Console.ForegroundColor = InputColor;
         String input = Console.ReadLine();
         Console.ForegroundColor = StandardColor;

         return input;
      }
      /// <summary>
      /// Show a colored message to the user
      /// </summary>
      /// <param name="info">The message to show to the user</param>
      /// <param name="noLine">Whether or not following console writes should appear on a new line</param>
      /// <returns></returns>
      void OutputToConsole(String info, Boolean noLine = false)
      {
         Console.ForegroundColor = OutputColor;
         if (noLine) Console.Write(info);
         else Console.WriteLine(info);
         Console.ForegroundColor = StandardColor;
      }
      /// <summary>
      /// Show a colored message to the user, giving standard coloring to the msg parameter.
      /// </summary>
      /// <param name="msg">The message to show to the user in standard coloring</param>
      /// <param name="info">The information to be provided</param>
      /// <param name="noLine">Whether or not a later message should appear on a new line</param>
      void OutputToConsole(String msg, String info, Boolean noLine = false)
      {
         Console.Write(msg);
         OutputToConsole(info, noLine);
      }
      void WarningToConsole(String msg, Boolean noLine = false)
      {
         Console.ForegroundColor = WarningColor;
         if (noLine) Console.Write(msg);
         else Console.WriteLine(msg);
         Console.ForegroundColor = StandardColor;
      }

   }
}
