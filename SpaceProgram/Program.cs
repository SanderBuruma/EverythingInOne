using System;
using Cubit32.Logging;
using Cubit32.Physics;
using SpaceProgram.Models.Entities;

namespace KerbalTradingProgram
{
   class Program
   {
      static void Main(string[] args)
      {
         try
         {
            StarSystem system = new StarSystem();
            Star star = new Star(
                velocity: new Vector2Decimal(),
                surfaceGravity: 25.2,
                position: new Vector2Decimal(),
                system,
                radius: 3e7,
                rotation: 0,
                angularMomentum: 0,
                name: "Sol"
            );
            star.ToConsole();
         }
         catch (Exception ex)
         {
            LoggingFormatter exFormatted = new LoggingFormatter(ex);
            Console.WriteLine(exFormatted.Subject + "\n\n" + exFormatted.Body);
         }

         Console.WriteLine("\n\nPress any key to exit...");
         Console.ReadKey();

      }
   }
}
