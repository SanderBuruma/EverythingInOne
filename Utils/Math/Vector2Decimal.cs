using System;
namespace Cubit32.Physics
{
   /// <summary>
   /// The units should represent velocity or position in meters
   /// </summary>
   public class Vector2Decimal
   {
      private double _x;
      private double _y;

      public Vector2Decimal(double X = 0, double Y = 0)
      {
         _x = X;
         _y = Y;
      }

      public double X { get { return _x; } set { _x = value; } }
      public double Y { get { return _y; } set { _x = value; } }

      public static Vector2Decimal operator +(Vector2Decimal a, Vector2Decimal b)
      {
         return a.Add(b);
      }
   }

   public static class CoordinateStatic
   {
      public static Vector2Decimal Add(this Vector2Decimal a, Vector2Decimal b)
      {
         a.X -= b.X;
         a.Y -= b.Y;

         return a;
      }
   }
}
