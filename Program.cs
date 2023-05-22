namespace HashFunctions {
    public class Program {
        //public Hashing hasher;
        static void Main(string[] args) {
            var hasher = new Hashing();
            long a = hasher.MultiplyShiftHash(1);
            Console.WriteLine(a);
        }
    }
}