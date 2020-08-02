using System;
using System.Collections.Generic;

namespace SpaceProgram.Models.Entities
{
   public class StarSystem
   {

#region Fields
      #endregion

      #region Constructor
      public StarSystem()
      {
         Planets = new List<Planet>();
         Ships = new List<Ship>();
      }
      #endregion

      #region Properties
      public Star Star { get; set; }
      public double TimeElapsed { get; set; }
      public List<Planet> Planets { get; set; }
      public List<Ship> Ships { get; set; }
      
      public List<Entity> AllEntities
      {
         get
         {
            List<Entity> list = new List<Entity>();
            list.Add(Star);
            list.AddRange(Planets);
            list.AddRange(Ships);
            return list;
         }
      }
      public List<CelestialBody> AllCelestialBodies
      {
         get
         {
            List<CelestialBody> list = new List<CelestialBody>();
            list.Add(Star);
            list.AddRange(Planets);
            return list;
         }
      }
      #endregion

      #region Methods
      public void AddPlanet(Planet planet)
      {
         Planets.Add(planet);
      }

      public void AddShip(Ship ship)
      {
         Ships.Add(ship);
      }

      public void LogAll()
      {
         foreach (Entity entity in AllEntities)
         {
            Console.WriteLine(
                String.Format
                (
                    "Name: {0}\n" +
                    "Type: {1}\n" +
                    "Mass: {2:#.###E+0} kg",
                    entity.Name,
                    entity.GetType(),
                    entity.Mass
                )
            );
         }
      }
      #endregion
   }
}
