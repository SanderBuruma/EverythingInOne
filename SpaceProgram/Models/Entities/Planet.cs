using System;
using Cubit32.Physics;

namespace SpaceProgram.Models.Entities
{
   public class Planet : CelestialBody
   {
      private StarSystem starSystem;

      public Planet(
          Vector2Decimal velocity,
          Vector2Decimal position,
          StarSystem starSystem,
          double mass = 1,
          double radius = 0.5,
          double rotation = 270,
          double angularMomentum = 0,
          CelestialBody influencingBody = null
      )
          : base
      (
          velocity: velocity,
          position: position,
          starSystem: starSystem,
          mass: mass,
          radius: radius,
          rotation: rotation,
          angularMomentum: angularMomentum,
          influencingBody: influencingBody
      )
      { }

      public Planet(
          Vector2Decimal velocity,
          double surfaceGravity,
          Vector2Decimal position,
          StarSystem starSystem,
          double radius = 0.5,
          double rotation = 270,
          double angularMomentum = 0,
          CelestialBody influencingBody = null
      )
          : base
      (
          velocity: velocity,
          position: position,
          starSystem: starSystem,
          rotation: rotation,
          angularMomentum: angularMomentum,
          mass: surfaceGravity * radius * radius / Physics.G,
          radius: radius,
          influencingBody: influencingBody
      )
      { }
   }
}
