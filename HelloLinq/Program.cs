using System;
using System.Collections.Generic;

namespace HelloLinq
{
    class Program
    {
        public static IEnumerable<Dog> Dogs => Dog.Repository.Values;

        private static List<Dog> ListDogsByNamePrefix(IEnumerable<Dog> dogs,
            string prefix)
        {
            var result = new List<Dog>();
            foreach (var dog in dogs)
            {
                if (dog.Name.StartsWith(prefix,
                    StringComparison.OrdinalIgnoreCase))
                    result.Add(dog);
            }
            return result;
        }

        private static string GetInput()
        {
            Console.Write("Írd be a kutya nevének elejét: ");
            var result = Console.ReadLine();
            Console.Clear();
            return result;
        }


        static void Main(string[] args)
        {
            string searchText;
            while ((searchText = GetInput()).Length > 0)
            {
                foreach (var dog in ListDogsByNamePrefix(Dogs, searchText))
                    Console.WriteLine(dog);
            }

        }
    }
}