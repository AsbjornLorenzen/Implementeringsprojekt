using System.Numerics;

namespace HashFunctions {

    public class CountSketch {

            public long[] CS() {

                var hasher = new Hashing();

                int t = 22; //m = 2^t
                int n = 5000000; // Size of data stream

                //Initialize the counter array with 2^t = m size
                long[] C = new long[(1 << t)];
                
                foreach (var tuple in Stream.CreateStream(n, t)) {
                    ulong x = tuple.Item1;
                    long d = tuple.Item2;

                    BigInteger k = hasher.PolynomialHash(x);
                    (ulong h, int s) = hasher.TwoHashFunctions(k, t);
                    C[h] += d * s;
                }
                
                return C;
            }

            public BigInteger estimateSquaredSum(long[] C) {
                BigInteger sum = 0;
                foreach (long i in C) {
                    sum += (long) Math.Pow(i, 2);
                }
                return sum;
            }


    }
}