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
                surfaceGravity: 28*9.81,
                position: new Vector3(),
                starSystem: system,
                radius: 6.96e8,
                rotation: 0,
                angularMomentum: new Vector3(),
                name: "Sol"
            );

            system.AddPlanet(new Planet(
               velocity: new Vector3(4e3f, 0f, 0f),
               surfaceGravity: 9.81,
               position: new Vector3(1e9f, 1e9f, 0),
               starSystem: system,
               radius: 6.371e6,
               name: "Earth"
            ));

            foreach (CelestialBody body in system.AllCelestialBodies)
            {
               body.ToConsole();
               body.PrintSurfaceGravity();
               Console.WriteLine();
            }

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
