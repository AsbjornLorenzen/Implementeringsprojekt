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
            hasher.CompareRunningTime(10000000);

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

