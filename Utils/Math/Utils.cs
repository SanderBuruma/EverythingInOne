using System;

namespace Cubit32.Numbers
{
   public static class DecimalExtensions
   {
      /// <summary>
      /// Returns the double square root of its input decimaal value
      /// </summary>
      /// <param name="x">the number being square rooted</param>
      /// <param name="guess">recursive parameter, leave blank</param>
      /// <returns></returns>
      public static decimal Sqrt(this decimal x, decimal? guess = null)
      {
         var ourGuess = guess.GetValueOrDefault(x / 2m);
         var result = x / ourGuess;
         var average = (ourGuess + result) / 2m;

         if (average == ourGuess) // This checks for the maximum precision possible with a double.
            return average;
         else
            return Sqrt(x, average);
      }

      /// <summary>
      /// Returns x to the power of 'pow', which is 2 by default.
      /// </summary>
      /// <param name="x">input</param>
      /// <param name="pow"> Must be greater than 0, is 2 by default</param>
      /// <returns></returns>
      public static decimal Pow(this decimal x, int pow = 2)
      {
         if (pow < 1) throw new ArgumentException("param pow was too small: {0}");
         decimal initialValue = x;
         while (pow-- > 0)
         {
            x *= initialValue;
         }
         return x;
      }
   }
}
