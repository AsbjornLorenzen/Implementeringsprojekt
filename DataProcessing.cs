using System.Diagnostics;
using System.Numerics;

namespace HashFunctions {
    using keyval = Tuple <ulong,long>;
    public class DataProcessing {
        int n, l;
        ulong universeSize;
        Hashfunction f;
        HashTable table;
        Hashing hasher;
        Stopwatch stopwatch;
        LinkedList<(ulong,int)> stream;
        // n is amount of keys to run, l is size of universe
        public DataProcessing(int n,int l,Hashfunction f, LinkedList<(ulong,int)> savedStream) {
            this.stream = savedStream;
            this.universeSize = (ulong) (1 << l);
            this.n = n;
            this.l = l;
            this.f = f;
            Debug.Assert((ulong) n >= this.universeSize);
            this.table = new HashTable(l,f);
            this.hasher = new Hashing();
            stopwatch = new Stopwatch();
        }


        // Streams elements and inserts them to hash table
        private void FillHashTable () {
            foreach (var tuple in this.stream) {
                ulong x = tuple.Item1;
                long d = tuple.Item2;
                this.table.increment(x,d);
            }
        }

        public BigInteger GetSquaredSum () {
            Console.WriteLine("Getting squared sum using {0}. Filling hash table...",this.f);
            stopwatch.Start();
            FillHashTable();
            stopwatch.Stop();
            Console.WriteLine("Filled hash table in {0}ms. Computing squared sum...",stopwatch.ElapsedMilliseconds);
            stopwatch.Reset();
            stopwatch.Start();
            BigInteger sum = 0;
            for (ulong k = 0;k < this.universeSize; k++) {
                LinkedList<keyval> node = this.table.getNode(k);
                foreach (keyval n in node) {
                    sum += (long) Math.Pow(n.Item2,2);
                }
            }
            stopwatch.Stop();
            Console.WriteLine("Computed sum in {0}ms. \n Sum was {1}. Params: l={2}, universeSize={4}, n={3}",stopwatch.ElapsedMilliseconds,sum,this.l,this.n,this.universeSize);
            return sum;
        }
    }
}