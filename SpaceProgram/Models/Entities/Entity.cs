using System;
using Cubit32.Numbers;
using Cubit32.Physics;

namespace SpaceProgram.Models.Entities
{
   public class Entity
   {
      #region fields
      private Vector2Decimal position;
      /// <summary>rad/s</summary>
      private double angularMomentum;
      private double mass;
      private double radius;
      ///<summary>0 is eastbound, 180 is west; 270 isn't north, it's up.</summary>
      private double rotation;
      private CelestialBody influencingBody;
      private Vector2Decimal velocityVector;
      private string name;
      private Orbit orbit;
      private StarSystem starSystem;
      #endregion

      #region Constructor
      public Entity
      (
          Vector2Decimal velocity,
          Vector2Decimal position,
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


      public double AngularMomentum
      {
         get => angularMomentum;
         set => angularMomentum = value;
      }

      /// <summary>
      /// Radius from influencing body's center of mass
      /// </summary>
      public double Altitude
      {
         get
         {
            return DistanceFIB - influencingBody.Radius;
         }
      }


      ///density of the entity in tons/m^3
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

      /// <summary>distance from influencing body</summary>
      public double DistanceFIB
      {
         get
         {
            return GetDistance(InfluencingBody);
         }
      }

      //local gravity minus centripetal acceleration
      public double EffectiveGravity
      {
         get
         {
            return LocalGravity - VelocityVector.X * VelocityVector.X / InfluencingBody.Radius;
         }
      }

      public CelestialBody InfluencingBody
      {
         get => influencingBody;
         set => influencingBody = value;
      }

      public double LocalGravity
      {
         get
         {
            double ratio = InfluencingBody.Radius / (InfluencingBody.Radius + Position.Y);
            return ratio * ratio * InfluencingBody.SurfaceGravity;
         }
      }

      public double Mass
      {
         get => mass;
         set => mass = value;
      }

      public string Name
      {
         get { return name; }
         set { name = value; }
      }

      /// <summary>
      /// Mass * GravitationalConstant
      /// </summary>
      public double Mu { get => Physics.G * Mass; }

      public Vector2Decimal Position
      {
         get => position;
         set => position = value;
      }

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

      public double Radius
      {
         get => radius;
         set => radius = value;
      }

      public double Rotation
      {
         get => rotation;
         set => rotation = value % (double)(2 * Math.PI);
      }

      public StarSystem StarSystem { get => starSystem; set => starSystem = value; }

      public double TimeElapsed { get => StarSystem.TimeElapsed; }

      public double Velocity { get => Math.Sqrt(VelocityVector.X * VelocityVector.X + VelocityVector.Y * VelocityVector.Y); }

      public Vector2Decimal VelocityVector { get => velocityVector; set => velocityVector = value; }
      #endregion

      #region Methods
      public void Move(Vector2Decimal change)
      {
         Position.X += change.X * InfluencingBody.Radius / Position.X;
         Position.Y += change.Y;
      }

      public void ChangeVelocity(Vector2Decimal change)
      {
         VelocityVector.Add(change);

         //todo proper vector positioning and velocity
         //increase or decrease horizontal velocity according to 
         if (change.Y > 0)
            VelocityVector.X = Math.Sqrt(change.X * change.X - change.Y * change.Y);
         else
            VelocityVector.X = Math.Sqrt(change.X * change.X + change.Y * change.Y);
      }

      /// <summary>
      /// Logs radius, name, mass, density and type of this body
      /// </summary>
      /// <param name="entity"></param>
      public void ToConsole(Entity entity)
      {
         Console.WriteLine(
             string.Format(
                 "Name            : \"{0}\"\n" +
                 "Typeof          : {1}\n" +
                 "Mass            : {2:#.###E+0} kg\n" +
                 "Radius          : {3:N0} km\n" +
                 "Density         : {3:##.##, 15} kg/dm^3",
                 entity.Name,
                 this.GetType(),
                 entity.Mass,
                 entity.Radius / 1e3,
                 entity.Density / 1e3
             )
         ); ;
      }

      public double GetDistance(Entity entity)
      {
         return Math.Sqrt(
             Math.Pow(Position.X - entity.position.X, 2) +
             Math.Pow(Position.Y - entity.position.Y, 2)
         );
      }
      #endregion
   }
}
