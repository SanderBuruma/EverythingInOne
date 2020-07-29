using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RandomBigPrime
{
   class Program
   {
      [STAThreadAttribute]
      static void Main()
      {
         //generate basic primes array with which to exclude the first batch of non-primes before submitting to the computationally intensive fermat test.
         var primes = new List<int>() { 2 };
         for (int i = 3; i < 2000000; i += 2)
         {
            foreach (var prime in primes)
            {
               if (prime * prime > i) break;
               if (i % prime == 0) goto goto1;
            }
            primes.Add(i);
            goto1:;
         }
         long primesCount = 0;

         while (true)
         {
            BigInteger p;
            var rng = new RNGCryptoServiceProvider();
            Console.Write("Input the number of bytes the prime number needs to have: ");
            int primeByteSize = int.Parse(Console.ReadLine());
            byte[] bytes = new byte[primeByteSize];

            BigInteger baseP = 2;
            for (int i = 2; i/8 < primeByteSize; i++)
            {
                  baseP *= 2;
            }
            
            var sw = new Stopwatch();
            sw.Start();

            goto2:;
            rng.GetBytes(bytes);
            p = baseP + BigInteger.Abs(new BigInteger(bytes) - 1);
            foreach (var item in primes)
            {
               if (p % item == 0) goto goto2;
            }

            if (CustomModExp(2, p - 1, p) == 1)
            {
               primesCount++;
               Console.WriteLine(p.ToString("N0") + "\n" + (((double)sw.ElapsedMilliseconds)/primesCount).ToString("N2"));
            }
            else
            {
               goto goto2;
            }
         }
      }

      static BigInteger CustomModExp(BigInteger a, BigInteger exponent, BigInteger modulus)
      {
         if (a < 2 || exponent < 2 || modulus < 2) throw new FormatException("an input is too small for CustomModExp()");
         var b = new BigInteger(1);
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
