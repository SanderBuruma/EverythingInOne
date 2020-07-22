using System;
using System.Collections.Generic;

namespace SpaceProgram.Models.Entities
{
   public class StarSystem
   {
      #region Fields
      private Star star;
      private List<Planet> planets;
      private List<Ship> ships;
      private double timeElapsed;
      #endregion

      #region Constructor
      public StarSystem()
      {
         Planets = new List<Planet>();
         Ships = new List<Ship>();
      }
      #endregion

      #region Properties
      public Star Star { get => star; set => star = value; }
      public double TimeElapsed { get => timeElapsed; set => timeElapsed = value; }
      public List<Planet> Planets { get => planets; set => planets = value; }
      public List<Ship> Ships { get => ships; set => ships = value; }

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
      #endregion

      #region Methods
      public void AddPlanet(Planet planet)
      {
         planet.InfluencingBody = Star;
         Planets.Add(planet);
      }

      public void AddShip(Ship ship)
      {
         ship.InfluencingBody = Star;
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
