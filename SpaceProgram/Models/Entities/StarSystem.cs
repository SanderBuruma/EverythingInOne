using System;
using System.Collections.Generic;

namespace SpaceProgram.Models.Entities
{
    public class StarSystem
    {
        public StarSystem(
            Star sol
        )
            : base()
        {
            Planets = new List<Planet>();
            Sol = sol;
        }

        public Star Sol { get; set; }
        public List<Planet> Planets { get; set; }

        public void AddPlanet(Planet planet)
        {
            Planets.Add(planet);
        }
    }
}
