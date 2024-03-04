using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Collections;

namespace IBMPonderThisMarch2024
{
    internal class Primality
    {
        private Hashtable cache = new Hashtable();
        public bool IsPrimeMillerRabin(BigInteger n)
        {
            if (cache.ContainsKey(n))
                return (bool)cache[n];

            //It does not work well for smaller numbers, hence this check
            int SMALL_NUMBER = 1000;

            if (n <= SMALL_NUMBER)
            {
                bool retVal = IsPrime(n);
                cache.Add(n, retVal);
                return retVal;
            }

            int MAX_WITNESS = 100;
            for (long i = 2; i <= MAX_WITNESS; i++)
            {
                if (IsPrime(i) && Witness(i, n) == 1)
                {
                    return false;
                }
            }

            cache.Add(n, true);
            return true;
        }

        private BigInteger SqRtN(BigInteger N)
        {
            /*++
             *  Using Newton Raphson method we calculate the
             *  square root (N/g + g)/2
             */
            BigInteger rootN = N;
            int count = 0;
            int bitLength = 1;
            while (rootN / 2 != 0)
            {
                rootN /= 2;
                bitLength++;
            }
            bitLength = (bitLength + 1) / 2;
            rootN = N >> bitLength;

            BigInteger lastRoot = BigInteger.Zero;
            do
            {
                if (lastRoot > rootN)
                {
                    if (count++ > 1000)                   // Work around for the bug where it gets into an infinite loop
                    {
                        return rootN;
                    }
                }
                lastRoot = rootN;
                rootN = (BigInteger.Divide(N, rootN) + rootN) >> 1;
            }
            while (!((rootN ^ lastRoot).ToString() == "0"));
            return rootN;
        }

        private bool IsPrime(BigInteger n)
        {
            if (n <= 1)
            {
                return false;
            }

            if (n == 2)
            {
                return true;
            }

            if (n % 2 == 0)
            {
                return false;
            }

            for (int i = 3; i <= SqRtN(n) + 1; i += 2)
            {
                if (n % i == 0)
                {
                    return false;
                }
            }
            return true;
        }

        private int Witness(long a, BigInteger n)
        {
            BigInteger t, u;
            BigInteger prev, curr = 0;
            BigInteger i;
            BigInteger lln = n;

            u = n / 2;
            t = 1;
            while (u % 2 == 0)
            {
                u /= 2;
                t++;
            }

            prev = BigInteger.ModPow(a, u, n);
            for (i = 1; i <= t; i++)
            {
                curr = BigInteger.ModPow(prev, 2, lln);
                if ((curr == 1) && (prev != 1) && (prev != lln - 1)) return 1;
                prev = curr;
            }
            if (curr != 1) return 1;
            return 0;
        }
    }
}
