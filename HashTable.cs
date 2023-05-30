using HashFunctions;
using System.Collections.Generic;
using System.Numerics;

namespace HashFunctions {
    using keyval = Tuple <ulong,long>;
    // Keys to the hashtables (x) are ulong, but d (weight) is long, so can be negative
    public enum Hashfunction {
        multiplymodprime,
        multiplyshifthash
    }
    public class HashTable {
        LinkedList<keyval>[] table;
        Hashing hasher;
        Hashfunction f;
        long size;
        int l;
        public HashTable(int l, Hashfunction f) {
            //Create array for all lists
            this.l = l;
            this.size = 1 << (l + 1);
            this.hasher = new Hashing();
            this.table = new LinkedList<keyval>[size];
            this.f = f;

            // Instantiate linked lists:
            for (int i = 0; i < size; i++) {
                table[i] = new LinkedList<keyval>();
            }
        }

        private ulong hash(ulong x, int l) {
            ulong key;
            switch (this.f) {
                case Hashfunction.multiplymodprime:
                    key = hasher.MultiplyModPrime(x,l);
                    break;
                case Hashfunction.multiplyshifthash:
                    key = hasher.MultiplyShiftHash(x,l);
                    break;
                default:
                    throw new Exception("Hashfunction must be specified");
            }
            return key;
        }

        public long get(ulong x) {
            ulong key = hash(x,this.l);
            foreach (var val in this.table[key]) {
                if (val.Item1 == x) {
                    return val.Item2;
                }
            }
            return 0;
        }

        public void set(ulong x, long v){
            ulong key = hash(x,this.l);
            keyval tuple = new keyval(x,v);

            // remove if x already in list
            foreach (keyval val in this.table[key]) {
                if (val.Item1 == x) {
                    this.table[key].Remove(val);
                    break;
                }
            }
            this.table[key].AddFirst(tuple);
        }

        public void increment(ulong x, long d){
            ulong key = hash(x,this.l);
            keyval newTuple = new keyval(x,d);

            foreach (keyval val in this.table[key]) {
                if (val.Item1 == x) {
                    newTuple = new keyval(x,val.Item2 + d);
                    this.table[key].Remove(val);
                    break;
                }
            }
            this.table[key].AddFirst(newTuple);
            return;
        }

        public LinkedList<keyval> getNode(ulong k) {
            return this.table[k];
        }
    }
}