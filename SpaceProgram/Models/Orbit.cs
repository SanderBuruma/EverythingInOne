using System;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Runtime.InteropServices.ComTypes;
using SpaceProgram.Models.Entities;

namespace SpaceProgram.Models
{
   public class Orbit
   {
      public Orbit(Entity entity, double timeElapsed)
      {
         TimeElapsed = timeElapsed;
         //calculate SMA
         SemiMajorAxis = //this isn't nearly as close as it should be to showing a proper math equation
             (entity.Mu * entity.DistanceFIB
                             /
             Math.Pow(entity.Velocity, 2) * entity.DistanceFIB) - (2 * entity.Mu);

         Entity = entity;
      }

      // a = mu*r / v^2 * r - 2 * mu 
      public double SemiMajorAxis { get; set; }
      public double Eccentricity
      {
         get
         {
            double xComponent = CalculateEccentricityComponent(
               Entity.VelocityVector,
               Entity.VelocityVector.X,
               Entity.InfluencingBody.Mu,
               Entity.Position,
               Entity.Position.X);
            double yComponent = CalculateEccentricityComponent(
               Entity.VelocityVector,
               Entity.VelocityVector.Y,
               Entity.InfluencingBody.Mu,
               Entity.Position,
               Entity.Position.Y);
            double zComponent = CalculateEccentricityComponent(
               Entity.VelocityVector,
               Entity.VelocityVector.Z,
               Entity.InfluencingBody.Mu,
               Entity.Position,
               Entity.Position.Z);

            return Math.Sqrt(Math.Pow(xComponent, 2) + Math.Pow(yComponent, 2) + Math.Pow(zComponent, 2));
         }
      }
      public double MeanAnomaly { get; set; }
      public double ArgumentOfPeriapsis { get; set; }
      public bool Retrograde { get; set; }
      public Entity Entity { get; set; }
      public double TimeElapsed { get; set; }

      /// <summary>
      /// Calculate one eccentricity component (x, y or z). paramsA should be the X Y or Z components.
      /// </summary>
      /// <param name="velocity">the velocity of the entity</param>
      /// <param name="u">Gravitational Constant * Planet Mass</param>
      /// <param name="VelocityA">the X Y or Z component of radius</param>
      /// <param name="position"></param>
      /// <param name="positionA">the X Y or Z component of radius</param>
      /// <returns></returns>
      private double CalculateEccentricityComponent(
         Vector3 velocity,
         double VelocityA,
         double u,
         Vector3 position,
         double positionA)
      {
         //where anything> is anything.X Y or Z

         //(v^2 - u/r)*r>
         double a = (velocity.LengthSquared() - (u / position.Length())) * positionA;
         //r> * v> * v>
         double b = positionA * VelocityA * VelocityA;
         return (a - b)/u;
      }
   }
}
