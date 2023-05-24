using System.Diagnostics;
using System.Numerics;

namespace HashFunctions {
    // Skal vores hashing funktioner kun tage unsigned, eller også signed integers?
    public class Hashing {
        ulong a;
        BigInteger a_mod, b_mod, p;
        public Hashing () {
            // Init values for multiplyshifthash:
            string a_binaryString = "0011101110011000101010101000110101000011111011010010000111010011";
            this.a = Convert.ToUInt64(a_binaryString, 2);
            Debug.Assert(this.a % 2 == 1,"a must be an odd number");

            // Init values for multiplymodprime:
            p = (BigInteger) 1 << 90 - 1; // mersenne prime 2^89 - 1;
            byte[] a_bytes = new byte[] {0xfc, 0x55, 0xce, 0x7e, 0x2a, 0x62, 0x29, 0x1d, 0xd7, 0x4b, 0xe5 };
            byte[] b_bytes = new byte[] {0x01, 0xac, 0xfc, 0xcc, 0x58, 0xbc, 0x3b, 0xef, 0x38, 0x77, 0x5d, 0xa1 };
            this.a_mod = new BigInteger(a_bytes);  // 01111110001010101110011100111111000101010011000100010100100011101110101110100101111100101
            this.b_mod = new BigInteger(b_bytes);  // 11010110011111100110011000101100010111100001110111110111100111000011101110101110110100001
            Debug.Assert(this.a_mod < this.p);
            Debug.Assert(this.b_mod < this.p);
        }

        public ulong MultiplyShiftHash (ulong x, int l) {
            int shiftAmount = 64 - (int) l;
            return ((this.a * x) >> shiftAmount);
        }

        public ulong MultiplyModPrime (ulong x, int l) {
            BigInteger t,y;
            Debug.Assert(x < (BigInteger) 1 << 179 - 2);

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

        // n is amount of values to be hashed
        public void CompareRunningTime (int n) {
            // TODO: We are using signed integers, so the stream is converted to signed numbers. Should we change this?
            // We could multiply by the second element in the tuple to get a random sign, if desired.
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