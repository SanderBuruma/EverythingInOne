using System;
using System.Numerics;
using Cubit32.Physics;

namespace SpaceProgram.Models.Entities
{
   public class Star : CelestialBody
   {
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
          Vector3 velocity,
          double surfaceGravity,
          Vector3 position,
          StarSystem starSystem,
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
          mass: surfaceGravity * radius * radius / Physics.G, //pass in mass based on surface gravity
          rotation: rotation,
          angularMomentum: angularMomentum,
          radius: radius,
          name: name
      )
      {
         StarSystem.Star = this;
      }
   }
}
