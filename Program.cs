namespace HashFunctions {
    public class Program {
        //public Hashing hasher;
        static void Main(string[] args) {
            var hasher = new Hashing();
            long a = hasher.MultiplyShiftHash(1,20);
            Console.WriteLine(a);
            hasher.MultiplyModPrime(5,20);
            hasher.CompareRunningTime(10000000);
        }
    }
}