using System;
using System.Collections.Generic;
using System.Numerics;
using Cubit32.Numbers;
using Cubit32.Physics;

namespace SpaceProgram.Models.Entities
{
   public class Entity
   {

      #region Fields
      ///<summary>0 is eastbound, 180 is west; 270 isn't north, it's up.</summary>
      private double rotation;
      #endregion

      #region Constructor
      public Entity
      (
          Vector3 velocity,
          Vector3 position,
          StarSystem starSystem,
          double rotation = 270,
          Vector3 angularMomentum = new Vector3(),
          double mass = 1,
          double radius = 0.5,
          string name = ""
      )
      {
         if (position.Y < 0)
            throw new Exception(String.Format("{0} was less than 0 and less and shouldn't have been", nameof(position) + ".Item2"));
         Position = position;
         Rotation = rotation;
         AngularMomentum = angularMomentum;
         Mass = mass;
         Radius = radius;
         VelocityVector = velocity;
         Name = name;
         StarSystem = starSystem;
      }
      #endregion

      #region Properties


      public Vector3 AngularMomentum { get; set; }

      /// <summary>density of the entity into g/cm^3</summary>
      public double Density
      {
         get
         {
            double volume = Math.Pow(Radius, 3) * Math.PI * 4d/3d;
            return Mass / volume;
         }
      }

      public double Diameter
      {
         get
         {
            return Math.PI * 2 * Radius;
         }
      }

      public double Mass { get; set; }

      public string Name { get; set; }

      /// <summary>
      /// Mass * GravitationalConstant
      /// </summary>
      public double Mu { get => Physics.G * Mass; }

      public Vector3 Position { get; set; }

      public double Radius { get; set; }

      public double Rotation
      {
         get => rotation;
         set => rotation = value % (double)(2 * Math.PI);
      }

      public StarSystem StarSystem { get; set; }

      public double TimeElapsed { get => StarSystem.TimeElapsed; }
      public string Type { get => this.GetType().Name; }

      public double Velocity { get => Math.Sqrt(VelocityVector.X * VelocityVector.X + VelocityVector.Y * VelocityVector.Y); }

      public Vector3 VelocityVector { get; set; }
      #endregion

      #region Methods
      public void Move(Vector3 change)
      {
         //todo: fix me
         //Position.X += change.X * InfluencingBody.Radius / Position.X;
         //Position.Y += change.Y;
      }

      public void ChangeVelocity(Vector3 change)
      {
         //todo implement 3d everything...
      }

      /// <summary>
      /// Logs radius, name, mass, density and type of this body
      /// </summary>
      /// <param name="entity"></param>
      public void ToConsole()
      {
         Console.WriteLine(
             string.Format(
                 "Name    : \"{0}\"\n" +
                 "Type    : {1}\n" +
                 "Mass    : {2:#.###E+0} kg\n" +
                 "Radius  : {3:N0} km\n" +
                 "Density : {4:N2} g/dm^3",
                 Name,
                 Type,
                 Mass,
                 Radius / 1e3,
                 Density / 1e3
             )
         ); ;
      }

      /// <summary>
      /// Get the current distance in meters to the target entity. Center of Mass to Center of Mass.
      /// </summary>
      /// <param name="target"></param>
      /// <returns></returns>
      public double GetDistance(Entity target)
      {
         return Math.Sqrt(
             Math.Pow(Position.X - target.Position.X, 2) +
             Math.Pow(Position.Y - target.Position.Y, 2)
         );
      }
      #endregion
   }
}
