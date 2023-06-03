using System.Numerics;

using System.Collections.Generic;

namespace HashFunctions {
    public class Program {
        //public Hashing hasher;
        static void Main(string[] args) {
            var hasher = new Hashing();

            // Test multiplymodprime and multiplyshifthash:
            // ulong a = hasher.MultiplyShiftHash(1,20);
            // Console.WriteLine(a);
            // hasher.MultiplyModPrime(5,20);
            // hasher.CompareRunningTime(1000000);

            // Test hash table: 
            // HashTable t = new HashTable(20,Hashfunction.multiplymodprime);
            // t.set(10000,10);
            // t.set(20000,20);
            // t.set(30000,30);
            // t.increment(10000,100);
            // long x = t.get(10000);
            // long y = t.get(20000);
            // long z = t.get(30000);
            // Console.WriteLine("Read x: {0} y: {1} z: {2}",x,y,z);

            // Test sum of squares:
            DataProcessing d = new DataProcessing(5000000,22,Hashfunction.multiplymodprime);
            d.GetSquaredSum();

            // Test polynomial hash:
            //ulong x = 119822238477;
            //BigInteger k = hasher.PolynomialHash(x);
            //Console.WriteLine("Hashed {0} to {1} using polynomial hashing",x,k);

            //int t = 20;
            //Tuple<ulong,int> hs = hasher.TwoHashFunctions(k,t);
            //Console.WriteLine("Hashed with two hashfunction and got {0}",hs);
            
            int n=100;
            int t=20;

            // Save the stream
            LinkedList<(ulong,int)> savedStream = new LinkedList<(ulong,int)>();
            foreach (var tuple in Stream.CreateStream(n, t)) {
                savedStream.AddFirst((tuple.Item1,tuple.Item2));
            }

            var count = new CountSketch(savedStream,t);
            long[] test = count.CS();
            BigInteger big = count.estimateSquaredSum(test);
            Console.WriteLine("Got estimated squared sum: {0}", big);

        }
    }
}