using System;
using System.Collections.Generic;
using System.Text;

namespace Parallel_Programming_Compulsory
{
    public class GetPrimesSequential
    {
        private static List<long> GetPrimeSequential(long first, long last)
        {
            long num;
            int i, counter;

            List<long> resultList = new List<long>();

            for (num = first; num <= last; num++)
            {
                counter = 0;

                for (i = 2; i <= num / 2; i++)
                {
                    if (num % i == 0)
                    {
                        counter++;
                        break;
                    }
                }

                if (counter == 0 && num != 1)
                    resultList.Add(num);
            }
            return resultList;
        }
    }
}
