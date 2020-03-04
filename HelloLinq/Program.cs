using System.Collections.Generic;

namespace HelloLinq
{
    class Program
    {
        public static IEnumerable<Dog> Dogs => Dog.Repository.Values;

        static void Main(string[] args)
        {
            
        }
    }
}