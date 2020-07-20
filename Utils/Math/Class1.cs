using System;

namespace Utils
{
    public static class DecimalExtensions
    {
        /// <summary>
        /// Returns the decimal square root of its input value
        /// </summary>
        /// <param name="x">the number being square rooted</param>
        /// <param name="guess">recursive parameter, leave blank</param>
        /// <returns></returns>
        public static decimal Sqrt(this decimal x, decimal? guess = null)
        {
            var ourGuess = guess.GetValueOrDefault(x / 2m);
            var result = x / ourGuess;
            var average = (ourGuess + result) / 2m;

            if (average == ourGuess) // This checks for the maximum precision possible with a decimal.
                return average;
            else
                return Sqrt(x, average);
        }
    }
}
