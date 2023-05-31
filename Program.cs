﻿using System.Numerics;

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
            // DataProcessing d = new DataProcessing(4000000,20,Hashfunction.multiplymodprime);
            // d.GetSquaredSum();

            // Test polynomial hash:
            ulong x = 119822238477;
            BigInteger k = hasher.PolynomialHash(x);
            Console.WriteLine("Hashed {0} to {1} using polynomial hashing",x,k);

            Tuple<ulong,int> hs = hasher.TwoHashFunctions(k,20);
            Console.WriteLine("Hashed with two hashfunction and got {0}",hs);

        }
    }
}