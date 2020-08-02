using System;
using System.Numerics;
using Cubit32.Physics;

namespace SpaceProgram.Models.Entities
{
   public class CelestialBody : Entity
   {
      public CelestialBody(
          Vector3 velocity,
          Vector3 position,
          StarSystem starSystem,
          double mass = 1,
          double radius = 0.5,
          double rotation = 270,
          Vector3 angularMomentum = new Vector3(),
          string name = ""
      )
          : base
      (
          velocity: velocity,
          position: position,
          starSystem: starSystem,
          rotation: rotation,
          angularMomentum: angularMomentum,
          mass: mass,
          radius: radius,
          name: name
      )
      { }

      public CelestialBody(
          Vector3 velocity,
          double surfaceGravity,
          Vector3 position,
          StarSystem starSystem,
          double radius = 0.5,
          double rotation = 270,
          Vector3 angularMomentum = new Vector3(),
          CelestialBody influencingBody = null,
          string name = "")
          : base
      (
          velocity: velocity,
          position: position,
          starSystem: starSystem,
          rotation: rotation,
          angularMomentum: angularMomentum,
          mass: surfaceGravity * radius * radius / Physics.G,
          radius: radius,
          name: name
      )
      { }

      public double SurfaceGravity
      {
         get
         {
            return Physics.G * Mass / (Radius * Radius);
         }
      }

      public void PrintSurfaceGravity()
      {
         Console.WriteLine(
             string.Format(
                 "Gravity : {0:N2} m/s^2", SurfaceGravity
             )
         );
      }

   }
}
