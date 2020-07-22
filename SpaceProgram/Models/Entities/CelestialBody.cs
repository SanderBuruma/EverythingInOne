using System;
using Cubit32.Physics;

namespace SpaceProgram.Models.Entities
{
   public class CelestialBody : Entity
   {
      public CelestialBody(
          Vector2Decimal velocity,
          Vector2Decimal position,
          StarSystem starSystem,
          double mass = 1,
          double radius = 0.5,
          double rotation = 270,
          double angularMomentum = 0,
          CelestialBody influencingBody = null,
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
          influencingBody: influencingBody,
          name: name
      )
      { }

      public CelestialBody(
          Vector2Decimal velocity,
          double surfaceGravity,
          Vector2Decimal position,
          StarSystem starSystem,
          double radius = 0.5,
          double rotation = 270,
          double angularMomentum = 0,
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
          influencingBody: influencingBody,
          name: name
      )
      { }

      public double SphereOfInfluence
      {
         get => Math.Pow(Mass / InfluencingBody.Mass, 2 / 5) * Orbit.SemiMajorAxis;
      }

      public double SurfaceGravity
      {
         get
         {
            return Physics.G * Mass / (Radius * Radius);
         }
      }
   }
}
