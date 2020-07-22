using System;
using SpaceProgram.Models.Entities;

namespace SpaceProgram.Models
{
   public class Orbit
   {
      private double timeElapsed;

      public Orbit(Entity entity, double timeElapsed)
      {
         TimeElapsed = timeElapsed;
         //calculate SMA
         SemiMajorAxis = //this isn't nearly as close as it should be to showing a proper math equation
             entity.Mu * entity.DistanceFIB
                             /
             Math.Pow(entity.Velocity, 2) * entity.DistanceFIB - 2 * entity.Mu;

         Entity = entity;
      }

      // a = mur / v^2r-2mu 
      public double SemiMajorAxis { get; set; }
      public double Eccentricity { get; set; }
      public double MeanAnomaly { get; set; }
      public double ArgumentOfPeriapsis { get; set; }
      public bool Retrograde { get; set; } = false;
      public Entity Entity { get; set; }
      public double TimeElapsed { get => timeElapsed; set => timeElapsed = value; }
   }
}
