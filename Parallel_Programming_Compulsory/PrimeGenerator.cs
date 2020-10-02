using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Parallel_Programming_Compulsory
{
    public class PrimeGenerator
    {
        public List<long> GetPrimesSequential(long first, long last)
        {
            List<long> resultList = new List<long>();
            BitArray bits = new BitArray((int)last + 1, true); // All true
            bits[0] = false;
            bits[1] = false;
            for (long i = 1; i * i <= last; i++) // Loops until the square root of the last number
            {
                if (bits[(int)i])
                {
                    for (long j = i * i; j <= last; j += i) // Removes all the products of the current number.
                    {
                        bits[(int)j] = false;
                    }
                }
            }

            for (int i = 0; i < bits.Length; i++) // BitArray to List<long>
            {
                if (bits[i])
                {
                    resultList.Add(i);
                }
            }

            return resultList.Where(x => x >= first).ToList();
        }

        public List<long> GetPrimesParallel(long first, long last)
        {
            var locker = new Object();
            IEnumerable<long> primes = new List<long>();
            Parallel.ForEach(
                Partitioner.Create(first, last),
                () => new List<long>(),
                (range, loopState, partialResult) =>
                {
                    for (long i = range.Item1; i < range.Item2; i++)
                    {
                        if (IsPrime((int)i))
                            partialResult.Add(i);
                    }
                    return partialResult;
                }, (partialResult) =>
                {
                    lock (locker)
                    {
                        primes = primes.Concat(partialResult);
                    }
                });
            return primes.OrderBy(s => s).ToList();
        }

        private bool IsPrime(int number)
        {
            if (number <= 1) return false;
            if (number == 2) return true;
            if (number % 2 == 0) return false;

            var boundary = (int)Math.Floor(Math.Sqrt(number));

            for (int i = 3; i <= boundary; i += 2)
                if (number % i == 0)
                    return false;

            return true;
        }
    }
}