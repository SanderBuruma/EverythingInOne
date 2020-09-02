using System;
using System.Numerics;
using Cubit32.Physics;

namespace SpaceProgram.Models.Entities
{
   public class Planet : CelestialBody
   {
      public Planet(
          Vector3 velocity,
          Vector3 position,
          StarSystem starSystem,
          string name,
          double mass = 1,
          double radius = 0.5,
          double rotation = 270,
          Vector3 angularMomentum = new Vector3()
      )
          : base
      (
          velocity: velocity,
          position: position,
          starSystem: starSystem,
          name: name,
          mass: mass,
          radius: radius,
          rotation: rotation,
          angularMomentum: angularMomentum
      )
      { }

      public Planet(
          Vector3 velocity,
          double surfaceGravity,
          Vector3 position,
          StarSystem starSystem,
          string name,
          double radius = 0.5,
          double rotation = 270,
          Vector3 angularMomentum = new Vector3()
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
          name: name
      )
      { }
   }
}
