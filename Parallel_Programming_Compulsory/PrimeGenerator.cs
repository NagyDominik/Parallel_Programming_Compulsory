using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Parallel_Programming_Compulsory
{
    public class PrimeGenerator
    {
        public List<long> GetPrimesSequential(long first, long last)
        {
            last = Int32.MaxValue - 1;
            List<long> resultList = new List<long>();
            BitArray bits = new BitArray((int)last + 1, true); // All true
            bits[0] = false;
            bits[1] = false;
            for (long i = first; i * i <= last; i++) // Loops until the square root of the last number
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

            return resultList;
        }
    }
}