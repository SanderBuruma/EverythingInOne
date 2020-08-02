using System;
using System.Numerics;
using Cubit32.Numbers;
using Cubit32.Physics;
using SpaceProgram.Enums;

namespace SpaceProgram.Models.Entities
{
   public class Ship : Entity
   {

      #region Fields
      private double thrustKn;
      #endregion

      #region Constructor
      public Ship
      (
          Vector3 velocity,
          Vector3 position,
          StarSystem starSystem,
          double dryMassKg,
          double exhVelocityMs,
          Planet currentPlanet,
          double radius,
          CelestialBody influencingBody,
          double fuelMassKg = 0,
          double thrustN = 0,
          Ship secondStage = null,
          int fuelFraction = 30,
          double rotation = 270,
          double angularMomentum = 0,
          string name = ""
      )
          : base
      (
          velocity: velocity,
          position: position,
          starSystem: starSystem,
          rotation: rotation,
          angularMomentum: angularMomentum,
          mass: dryMassKg * (1 + 1 / fuelFraction),
          radius: radius,
          influencingBody: influencingBody,
          name: name
      )
      {
         if (dryMassKg <= 0)
            throw new Exception(String.Format("{0} param was 0 or less and shouldn't have been", nameof(dryMassKg)));
         if (fuelMassKg < 0)
            throw new Exception(String.Format("{0} less than 0 and shouldn't have been", nameof(fuelMassKg)));
         if (thrustN < 0)
            throw new Exception(String.Format("{0} less than 0 and less and shouldn't have been", nameof(thrustN)));
         if (exhVelocityMs <= 0)
            throw new Exception(String.Format("{0} param was 0 or less and shouldn't have been", nameof(exhVelocityMs)));
         if (fuelFraction <= 0)
            throw new Exception(String.Format("{0} param was 0 or less and shouldn't have been", nameof(fuelFraction)));

         FuelMass = fuelMassKg;
         ExhaustVelocity = exhVelocityMs;
         Thrust = thrustN;
         Status = ShipStatus.Landed;
         InfluencingBody = currentPlanet;
         SecondStage = secondStage;
      }
      #endregion

      #region Properties
      //makes a cler distinction between this and fuelmass
      public double DryMass { get { return Mass; } }
      public double ExhaustVelocity { get; set; }
      public double FuelMass { get; set; }
      public Ship SecondStage { get; set; }
      public ShipStatus Status { get; set; }
      public double Thrust { get { return thrustKn; } set => thrustKn = value; }

      public double Acceleration { get { return Thrust / TotalMass; } }

      public double FuelUse { get { return Thrust / ExhaustVelocity; } }

      public double TotalMass
      {
         get
         {
            if (SecondStage == null) return FuelMass + DryMass;
            return FuelMass + DryMass + SecondStage.TotalMass;
         }
      }

      public double TWR { get { return Acceleration / EffectiveGravity; } }
      #endregion

      #region Methods
      #endregion
   }
}
