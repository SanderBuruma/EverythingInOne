using System;
using SpaceProgram.Models.Entities;

namespace SpaceProgram.Models
{
   public class Orbit
   {
      private double semiMajorAxis;
      private double eccentricity;
      private double meanAnomaly;
      private double argumentOfPeriapsis;
      private bool retrograde = false;
      private Entity entity;
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
      public double SemiMajorAxis { get => semiMajorAxis; set => semiMajorAxis = value; }
      public double Eccentricity { get => eccentricity; set => eccentricity = value; }
      public double MeanAnomaly { get => meanAnomaly; set => meanAnomaly = value; }
      public double ArgumentOfPeriapsis { get => argumentOfPeriapsis; set => argumentOfPeriapsis = value; }
      public bool Retrograde { get => retrograde; set => retrograde = value; }
      public Entity Entity { get => entity; set => entity = value; }
      public double TimeElapsed { get => timeElapsed; set => timeElapsed = value; }
   }
}
