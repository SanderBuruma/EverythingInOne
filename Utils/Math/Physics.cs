using System;

namespace Cubit32.Physics
{
    public static class Physics
    {
        private static decimal _gravitationalConstant = 6.67430e-11m;

        public static decimal G
        {
            get
            {
                return _gravitationalConstant;
            }
        }

        /// <param name="bodyMass">the mass in kilograms of the body generating the gravity</param>
        /// <param name="radius">In meters</param>
        /// <returns>m/s^2</returns>
        public static decimal GetGravity(decimal bodyMass, decimal radius)
        {
            return bodyMass * G / radius / radius;
        }
    }
}
