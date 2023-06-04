using System.Numerics;

namespace HashFunctions {

    public class CountSketch {
        
        LinkedList<(ulong,int)> stream;
        int t;
        Hashing hasher;
        public CountSketch(LinkedList<(ulong,int)> staticStream, int t, Hashing givenHasher) {
            this.stream = staticStream;
            this.t = t;
            this.hasher = givenHasher;
        }

        public long[] CS() {

            //Initialize the counter array with 2^t = m size
            long[] C = new long[(1 << t)];
            
            foreach (var tuple in this.stream) {
                ulong x = tuple.Item1;
                long d = tuple.Item2;

                BigInteger k = this.hasher.PolynomialHash(x);
                (ulong h, int s) = this.hasher.TwoHashFunctions(k, this.t);
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