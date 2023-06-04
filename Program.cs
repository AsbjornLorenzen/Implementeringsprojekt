using System.Numerics;
using System.Text.Json;
using System.Collections.Generic;
using System.Diagnostics;

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
            
            ///DataProcessing d = new DataProcessing(5000000,22,Hashfunction.multiplymodprime);
            ///d.GetSquaredSum();

            // Test polynomial hash:
            //ulong x = 119822238477;
            //BigInteger k = hasher.PolynomialHash(x);
            //Console.WriteLine("Hashed {0} to {1} using polynomial hashing",x,k);

            //int t = 20;
            //Tuple<ulong,int> hs = hasher.TwoHashFunctions(k,t);
            //Console.WriteLine("Hashed with two hashfunction and got {0}",hs);
            

            int n=(int) Math.Pow(10,7);
            int l = 23; // for number of different keys in datastream
            // Save the stream
            LinkedList<(ulong,int)> savedStream = new LinkedList<(ulong,int)>();
            foreach (var tuple in Stream.CreateStream(n, l)) {
                savedStream.AddFirst((tuple.Item1,tuple.Item2));
            }

            int[] ts = {23, 21, 19};
            int iteration = 1;
            foreach (int t in ts) {
                resultsClass results = new resultsClass();

                DataProcessing d = new DataProcessing(n,t,Hashfunction.multiplymodprime, savedStream);
                int trueSquaredSum = (int) d.GetSquaredSum();
                results.trueSum = trueSquaredSum;

                hasher.restartRandomUpdate();
                var count = new CountSketch(savedStream,t,hasher);
                long[] test;
                int big;

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                stopwatch.Stop();
                for (int i = 0; i<100;i++){
                    hasher.updateRandomFourUniversal();
                    
                    //run and time count sketch
                    stopwatch.Restart();
                    test = count.CS();
                    stopwatch.Stop();
                    results.times[i] = (int) stopwatch.ElapsedMilliseconds;

                    big = (int) count.estimateSquaredSum(test);
                    Console.WriteLine("{0} Estimated squared sum: {1}",i, big);
                    results.estimates[i]=big;
                }
                results.t = t;
                string jsonString = JsonSerializer.Serialize(results);
                string jsonFileName = "est"+iteration.ToString() + ".json";
                File.WriteAllText(jsonFileName, jsonString);
                Console.WriteLine(results.estimates[0]);
                iteration += 1;
            }
            


        }

        public class resultsClass {
            public int[] estimates { get; set; } = new int[100];
            public int[] times { get; set; } = new int[100];
            public int trueSum { get; set; } = 0;
            public int t { get; set; } = 0;
        }
    }
}

