using System;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cubit32.Numbers;

namespace Cubit32.Primes
{
   public static class Primes
   {
      public static int[] _primes;

      public static void Initialize()
      {
         List<int> primes = new List<int>() { 2 };
         for (int i = 3; i < 2000000; i += 2)
         {
            foreach (int prime in primes)
            {
               if (prime * prime > i) break;
               if (i % prime == 0) goto goto1;
            }
            primes.Add(i);
            goto1:;
         }
         _primes = primes.ToArray();

      }
      /// <summary>
      /// Generates a big prime number with the target nr of digits
      /// </summary>
      /// <returns></returns>
      public static BigInteger Generate(int digits, CancellationToken cToken)
      {
         //do not permit too large requests
         if (digits < 6) return new BigInteger();
         if (digits > 1000) return new BigInteger();

         BigInteger p;
         RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
         int primeByteSize = digits * 101/241;
         byte[] bytes = new byte[primeByteSize];

         BigInteger baseP = 2;
         for (int i = 2; i/8 < primeByteSize; i++)
         {
               baseP *= 2;
         }
            

         goto2:;
         if (cToken.IsCancellationRequested) return new BigInteger(131);
         rng.GetBytes(bytes);
         p = baseP + BigInteger.Abs(new BigInteger(bytes) - 1);
         foreach (int prime in _primes)
         {
            if (p % prime == 0) goto goto2;
         }

         if (CustomModExp(2, p - 1, p) == 1)
         {
            return p;
         }
         else
         {
            goto goto2;
         }
      }
      
      /// <summary>
      /// Just about as efficient as the original in buiilt function
      /// </summary>
      /// <returns></returns>
      public static BigInteger CustomModExp(BigInteger a, BigInteger exponent, BigInteger modulus)
      {
         if (a < 2 || exponent < 2 || modulus < 2) throw new FormatException("an input is too small for CustomModExp()");
         BigInteger b = new BigInteger(1);
         while (true)
         {
            if (exponent % 2 == 1)
            {
               b *= a;
               b %= modulus;
            }

            a *= a;
            a %= modulus;
            exponent /= 2;
            if (exponent <= 0) break;
         }
         return b;
      }
   }
}
