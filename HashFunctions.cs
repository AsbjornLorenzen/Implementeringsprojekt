using System.Diagnostics;
using System.Numerics;

namespace HashFunctions {
    // Skal vores hashing funktioner kun tage unsigned, eller også signed integers?
    public class Hashing {
        ulong a;
        BigInteger a_mod, b_mod, p, a0, a1, a2, a3;
        BigInteger[] a_arr;

        public Hashing () {
            // Init values for multiplyshifthash:
            string a_binaryString = "0011101110011000101010101000110101000011111011010010000111010011";
            this.a = Convert.ToUInt64(a_binaryString, 2);

            // Init values for multiplymodprime:
            p = (BigInteger) ((BigInteger) 1 << 89) - 1; // mersenne prime 2^89 - 1;
            byte[] a_bytes = new byte[] {0xff, 0x55, 0xce, 0x7e, 0x2a, 0x62, 0x29, 0x1d, 0xd7, 0x4b, 0xe5, 0x01};
            byte[] b_bytes = new byte[] {0xac, 0xfc, 0xcc, 0x58, 0xbc, 0x3b, 0xef, 0x38, 0x77, 0x5d, 0xa1 };
            this.a_mod = new BigInteger(a_bytes, isUnsigned: true); 
            this.b_mod = new BigInteger(b_bytes, isUnsigned: true);

            // Init values for 4-universal hashfunction:
            byte[] a0_bytes = new byte[] {0x74, 0xb9, 0x45, 0x5b, 0x5f, 0x5e, 0x96, 0x14, 0xde, 0x68, 0x57, 0x01};
            byte[] a1_bytes = new byte[] {0xd2, 0xe8, 0xd2, 0x16, 0x61, 0x01, 0x10, 0x3c, 0x4f, 0x9f, 0xc8, 0x01};
            byte[] a2_bytes = new byte[] {0x46, 0x44, 0x7b, 0xe2, 0x70, 0x96, 0x4e, 0x3e, 0xae, 0x85, 0x40, 0x01};
            byte[] a3_bytes = new byte[] {0x97, 0xa3, 0xcc, 0x3b, 0x22, 0x5d, 0x0e, 0xf6, 0x17, 0xa4, 0xe2};
            this.a0 = new BigInteger(a0_bytes, isUnsigned: true);
            this.a1 = new BigInteger(a1_bytes, isUnsigned: true);
            this.a2 = new BigInteger(a2_bytes, isUnsigned: true);
            this.a3 = new BigInteger(a3_bytes, isUnsigned: true);
            this.a_arr = new BigInteger[] {this.a0,this.a1,this.a2,this.a3};

            testInitialization();
        }

        private void testInitialization () {
            // Multiplyshifthash:
            Debug.Assert(this.a % 2 == 1,"a must be an odd number");

            // Multiplymodprime:
            Debug.Assert(0 <= this.a_mod);
            Debug.Assert(0 <= this.b_mod);
            Debug.Assert(this.a_mod < this.p);
            Debug.Assert(this.b_mod < this.p);

            // 4-universal hash:
            Debug.Assert(this.a0 < this.p);
            Debug.Assert(this.a1 < this.p);
            Debug.Assert(this.a2 < this.p);
            Debug.Assert(this.a3 < this.p);
            Debug.Assert(0 <= this.a0);
            Debug.Assert(0 <= this.a1);
            Debug.Assert(0 <= this.a2);
            Debug.Assert(0 <= this.a3);
        }

        public ulong MultiplyShiftHash (ulong x, int l) {
            int shiftAmount = 64 - (int) l;
            return ((this.a * x) >> shiftAmount);
        }

        public ulong MultiplyModPrime (ulong x, int l) {
            BigInteger t,y;
            Debug.Assert(x < (BigInteger) 1 << 178 - 2);

            // Efficient (ax + b) mod p
            t = (this.a_mod * x + this.b_mod);
            y = (t & this.p) + (t >> 89);
            if (y >= this.p) {
                y -= this.p;
            }

            // Efficient mod 2^l
            uint bitmask = (uint) (1UL << ((int) l + 1)) - 1;
            return (ulong) (y & bitmask);
        }

        public BigInteger PolynomialHash (ulong x) {
            // 4-universal hashing function
            // calculates a0 + a1x + a2x^2 + a3x^3 efficiently using Horner's rule
            BigInteger bigx = new BigInteger(x);
            BigInteger y = this.a_arr[3];
            for (int i = 2; i>=0; i--) {
                y = y * bigx + this.a_arr[i];
                y = (y & this.p) + (y >> 89);
            }
            if (y >= this.p) {y -= this.p;}
            return y;
        }

        // n is amount of values to be hashed
        public void CompareRunningTime (int n) {
            BigInteger sum = 0;
            int l = 28; // l is a random number less than 64
            Stopwatch stopwatch = new Stopwatch();
            
            stopwatch.Start();

            foreach (var tuple in Stream.CreateStream(n,l)) {
                ulong x = tuple.Item1;
                sum += MultiplyShiftHash(x,l);
            }

            stopwatch.Stop();
            Console.WriteLine("Hashed {0} numbers with MultiplyShiftHashing in {1} milliseconds",n,stopwatch.ElapsedMilliseconds);
            sum = 0;
            stopwatch.Restart();

            foreach (var tuple in Stream.CreateStream(n,l)) {
                ulong x = tuple.Item1;
                sum += MultiplyModPrime(x,l);
            }

            stopwatch.Stop();
            Console.WriteLine("Hashed {0} numbers with MultiplyModPrime in {1} milliseconds",n,stopwatch.ElapsedMilliseconds);
        }
    }
}