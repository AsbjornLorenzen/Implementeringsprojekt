using HashFunctions;
using System.Collections.Generic;
using System.Numerics;

namespace HashFunctions {
    using keyval = Tuple <BigInteger,long>;
    
    public class HashTable {
        LinkedList<keyval>[] table;
        Hashing hasher;
        long size;
        uint l;
        public HashTable(uint l) {
            //Create array for all lists
            this.l = l;
            this.size = 1 << (int) (l + 1);
            this.hasher = new Hashing();
            this.table = new LinkedList<keyval>[size];

            // Instantiate linked lists:
            for (int i = 0; i < size; i++) {
                table[i] = new LinkedList<keyval>();
            }
        }
        
        public long get(BigInteger x) {
            ulong key = this.hasher.MultiplyModPrime(x,this.l);
            foreach (var val in this.table[key]) {
                if (val.Item1 == x) {
                    return val.Item2;
                }
            }
            return 0;
        }

        public void set(BigInteger x, long v){
            ulong key = this.hasher.MultiplyModPrime(x,this.l);
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

        public void increment(BigInteger x, long d){
            ulong key = this.hasher.MultiplyModPrime(x,this.l);
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
    }
}