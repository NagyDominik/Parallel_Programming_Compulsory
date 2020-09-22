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

            return resultList.Skip((int)first).ToList();
        }

        public List<long> GetPrimesParallel(long first, long last)
        {
            List<long> resultList = new List<long>();
            BitArray bits = new BitArray((int)last + 1, true); // All true
            bits[0] = false;
            bits[1] = false;

            for (int i = 1; i * i <= last; i++) // Loops until the square root of the last number
            {
                if (bits[(int)i])
                {
                    Parallel.ForEach(Partitioner.Create(i, last), (range) =>
                    {
                        BitArray temp = new BitArray((int)last + 1, true);
                        for (long j = range.Item1 * range.Item1; j <= last; j += range.Item1) // Removes all the products of the current number.
                        {
                            temp[(int)j] = false;
                        }
                        lock (this)
                        {
                            bits.And(temp);
                        }
                    });
                }
            }

            for (int i = 0; i < bits.Length; i++) // BitArray to List<long>
            {
                if (bits[i])
                {
                    resultList.Add(i);
                }
            }

            return resultList.Skip((int)first).ToList();
        }
    }
}