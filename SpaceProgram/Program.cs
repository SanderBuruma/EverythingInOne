using System;
using System.Numerics;
using Cubit32.Logging;
using Cubit32.Physics;
using SpaceProgram.Models;
using SpaceProgram.Models.Entities;

namespace KerbalTradingProgram
{
   class Program
   {
      static void Main()
      {
         try
         {
            StarSystem system = new StarSystem();
            Star star = new Star(
                velocity: new Vector3(),
                surfaceGravity: 25.2,
                position: new Vector3(),
                system,
                radius: 3e7,
                rotation: 0,
                angularMomentum: new Vector3(),
                name: "Sol"
            );
            star.ToConsole();

            system.AddPlanet(new Planet(
                  new Vector3(4e3f, 0f, 0f),
                  9.81,
                  new Vector3(1e9f, 1e9f, 0),
                  system,
                  radius: 6.35e6
               )
            );

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
