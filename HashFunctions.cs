using System.Diagnostics;
using System.Numerics;

namespace HashFunctions {
    public class Hashing {
        Int64 a;
        int l;
        public Hashing () {
            string a_binaryString = "0011101110011000101010101000110101000011111011010010000111010011";
            this.a = Convert.ToInt64(a_binaryString, 2);
            this.l = 63;
            Debug.Assert(this.l < 64,"l must be less than 64");
            Debug.Assert(this.a % 2 == 1,"a must be an odd number");
        }

        public Int64 MultiplyShiftHash (Int64 x) {
            int shiftAmount = 64 - this.l;
            return ((this.a * x) >> shiftAmount);
        }

        public BigInteger MultiplyModPrime (BigInteger x) {
            // For optimization (if this is repeated often), we should move these assignments to the constructur of the class, rather than performing them at each method call
            BigInteger a,b,p,t,y;
            p = (BigInteger) 1 << 90 - 1; // mersenne prime 2^89 - 1;
            byte[] a_bytes = new byte[] {0xfc, 0x55, 0xce, 0x7e, 0x2a, 0x62, 0x29, 0x1d, 0xd7, 0x4b, 0xe5 };
            byte[] b_bytes = new byte[] {0x01, 0xac, 0xfc, 0xcc, 0x58, 0xbc, 0x3b, 0xef, 0x38, 0x77, 0x5d, 0xa1 };
            a = new BigInteger(a_bytes);  // 01111110001010101110011100111111000101010011000100010100100011101110101110100101111100101
            b = new BigInteger(b_bytes);  // 11010110011111100110011000101100010111100001110111110111100111000011101110101110110100001

            Debug.Assert(a < p);
            Debug.Assert(b < p);
            Debug.Assert(x < (BigInteger) 1 << 179 - 2);

            // Efficient (ax + b) mod p
            t = (a * x + b);
            y = (t & p) + (t >> 89);
            if (y >= p) {
                y -= p;
            }

            // Efficient mod 2^l
            int bitmask = (1 << (this.l + 1)) - 1;
            return y & bitmask;
        }
    }
}