using System;
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
      private Orbit orbit;
      #endregion

      #region Constructor
      public Entity
      (
          Vector3 velocity,
          Vector3 position,
          StarSystem starSystem,
          double rotation = 270,
          double angularMomentum = 0,
          double mass = 1,
          double radius = 0.5,
          CelestialBody influencingBody = null,
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
         InfluencingBody = influencingBody;
         VelocityVector = velocity;
         Name = name;
         StarSystem = starSystem;
         if (influencingBody != null)
            orbit = new Orbit(this, TimeElapsed);
      }
      #endregion

      #region Properties


      public double AngularMomentum { get; set; }

      /// <summary>Radius from influencing body's center of mass</summary>
      public double Altitude
      {
         get
         {
            return DistanceFIB - InfluencingBody.Radius;
         }
      }

      /// <summary>density of the entity in tons/m^3</summary>
      public double Density
      {
         get
         {
            return Mass / (Radius * Radius * Radius * 4 / 3);
         }
      }

      public double Diameter
      {
         get
         {
            return Math.PI * 2 * Radius;
         }
      }

      ///<summary>distance from influencing body</summary>
      public double DistanceFIB
      {
         get
         {
            return GetDistance(InfluencingBody);
         }
      }

      ///<summary>local gravity minus centripetal acceleration</summary>
      public double EffectiveGravity
      {
         get
         {
            return LocalGravity - VelocityVector.X * VelocityVector.X / InfluencingBody.Radius;
         }
      }

      //todo infer this from a findInfluencingBody property in StarSystem
      public CelestialBody InfluencingBody { get; set; }

      public double LocalGravity
      {
         get
         {
            double ratio = InfluencingBody.Radius / (InfluencingBody.Radius + Position.Y);
            return ratio * ratio * InfluencingBody.SurfaceGravity;
         }
      }

      public double Mass { get; set; }

      public string Name { get; set; }

      /// <summary>
      /// Mass * GravitationalConstant
      /// </summary>
      public double Mu { get => Physics.G * Mass; }


      public Orbit Orbit
      {
         get
         {
            if (orbit.TimeElapsed != StarSystem.TimeElapsed)
            {
               orbit = new Orbit(this, StarSystem.TimeElapsed);
            }
            return orbit;
         }
         set => orbit = value;
      }
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
                 "Density : {3:N2} kg/dm^3",
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
