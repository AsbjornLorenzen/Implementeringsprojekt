using System.Diagnostics;

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
    }
}