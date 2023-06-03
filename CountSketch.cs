using System.Numerics;

namespace HashFunctions {

    public class CountSketch {
        
        LinkedList<(ulong,int)> stream;
        int t;
        public CountSketch(LinkedList<(ulong,int)> staticStream, int t) {
            this.stream = staticStream;
            this.t = t;
        }

        public long[] CS() {

            var hasher = new Hashing();


            //Initialize the counter array with 2^t = m size
            long[] C = new long[(1 << t)];
            
            foreach (var tuple in this.stream) {
                ulong x = tuple.Item1;
                long d = tuple.Item2;

                BigInteger k = hasher.PolynomialHash(x);
                (ulong h, int s) = hasher.TwoHashFunctions(k, this.t);
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