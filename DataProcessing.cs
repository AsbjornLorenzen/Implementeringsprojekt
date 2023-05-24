using System.Diagnostics;
using System.Numerics;

namespace HashFunctions {
    public class DataProcessing {
        int n, l;
        HashTable table;
        // n is amount of keys to run, l is size of universe
        public DataProcessing(int n,int l) {
            this.n = n;
            this.l = l;
            this.table = new HashTable(l);
        }


    }
}