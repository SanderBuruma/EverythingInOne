using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimesSieve
{
   class Program
   {
      static void Main()
      {

         while (true)
         {
            var sw = new Stopwatch();
            Console.Write("Input the maximum number up to which to sieve for primes (this program wont go higher than 1 billion): ");

            uint maxPrime = Math.Min(200000000u,Math.Max(100,uint.Parse(Console.ReadLine())));

            sw.Start();
            var primes = new uint[maxPrime]; //uinteger "bool" values, 0 or 1 for false and true
            for (uint i = 0; i < primes.Length; i++)
            {
                  primes[i] = 1;
            }
            primes[0] = 0;
            primes[1] = 0;
            for (uint i = 0; i < primes.Length; i++)
            {
               if (primes[i] == 0) continue;
               if (i * i > primes.Length) break;
               for (uint j = i*2; j < primes.Length; j += i)
               {
                  primes[j] = 0;
                  //skip the loop check for a while if we have a long way to go anyway.
                  if (primes.Length < j + i * 20) continue;
                  j += i;
                  primes[j += i] = 0;
                  primes[j += i] = 0;
                  primes[j += i] = 0;
                  primes[j += i] = 0;
                  primes[j += i] = 0;
                  primes[j += i] = 0;
                  primes[j += i] = 0;
                  primes[j += i] = 0;
                  primes[j += i] = 0;
                  primes[j += i] = 0;
                  primes[j += i] = 0;
                  primes[j += i] = 0;
                  primes[j += i] = 0;
                  primes[j += i] = 0;
                  primes[j += i] = 0;
                  primes[j += i] = 0;
                  primes[j += i] = 0;
                  primes[j += i] = 0;
                  primes[j += i] = 0;

               }
            }
            Console.WriteLine((sw.ElapsedMilliseconds).ToString("N0") + " milliseconds used to calculate the primes");
            sw.Restart();
            uint primesCount = 0;
            foreach (uint v in primes)
            {
               primesCount+=v;
            }
            Console.WriteLine(primesCount.ToString("N0") + " primes detected");
            Console.WriteLine((sw.ElapsedMilliseconds).ToString("N0") + " milliseconds used to count the primes");
            Console.ReadKey();
         }
      }
   }
}
