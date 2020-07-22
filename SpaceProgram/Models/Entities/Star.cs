using System;
using Cubit32.Physics;

namespace SpaceProgram.Models.Entities
{
   public class Star : CelestialBody
   {
      private StarSystem starSystem;

      public Star
      (
          CelestialBody cb
      )
          : base
      (
          cb.VelocityVector,
          cb.SurfaceGravity,
          cb.Position,
          cb.StarSystem,
          cb.Radius,
          cb.Rotation,
          cb.AngularMomentum
      )
      { }

      public Star
      (
          Vector2Decimal velocity,
          double surfaceGravity,
          Vector2Decimal position,
          StarSystem starSystem,
          double radius = 0.5,
          double rotation = 270,
          double angularMomentum = 0,
          string name = ""
      )
          : base
      (
          velocity: velocity,
          position: position,
          starSystem: starSystem,
          mass: surfaceGravity * radius * radius / Physics.G,
          rotation: rotation,
          angularMomentum: angularMomentum,
          radius: radius,
          name: name
      )
      { }
   }
}
