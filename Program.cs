using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Collections;

namespace IBMPonderThisMarch2024
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Hashtable sequence = new Hashtable(); //Holds the key which is the sequence size N, and the A0
            Primality primality = new Primality();

            long seed = 1;
            int TARGET_KEY = 1000;
            long lastCount = 0;
            while (!sequence.ContainsKey(TARGET_KEY))
            {
                long jump = SequencesSeededBy(seed, sequence, primality, TARGET_KEY);
                seed = (seed == jump) ? seed + 1 : jump;
                if (sequence.Count != lastCount)
                {
                    Console.WriteLine("Found {0} sequences", sequence.Count);
                    PrintA0(sequence, sequence.Count);
                    lastCount = sequence.Count;
                }
            }
            PrintA0(sequence, TARGET_KEY);
        }

        static long SequencesSeededBy(long seed,
                                      Hashtable sequence,
                                      Primality primality,
                                      int TARGET_KEY)
        {
            long retVal = seed;
            List<long> list = new List<long>();

            list.Add(seed);

            //Special case for 1
            if (!sequence.ContainsKey(list.Count))
            {
                sequence.Add(list.Count, list[0]);
                retVal = list[list.Count - 1];
            }

            if (!primality.IsPrimeMillerRabin(seed)) //A0
            {
                long index = 1;
                seed += index;
                while (!primality.IsPrimeMillerRabin(seed))
                {
                    list.Add(seed);
                    if (!sequence.ContainsKey(list.Count))
                    {
                        sequence.Add(list.Count, list[0]);
                        retVal = list[list.Count - 1];

                        if (list.Count == TARGET_KEY)
                            break;
                    }
                    index++;
                    seed += index;
                }
            }

            return retVal;
        }

        static void PrintA0(Hashtable sequence,
                            int key)
        {
            Console.WriteLine("X{0}: A0 = {1}", key, (long)sequence[key]);
        }
    }
}
