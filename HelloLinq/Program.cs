
using HelloLinq;

static List<Dog> ListDogsByNamePrefix(IEnumerable<Dog> dogs,
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

static string GetInput()
{
    Console.Write("Írd be a kutya nevének elejét: ");
    var result = Console.ReadLine();
    Console.Clear();
    return result;
}

string searchText;
IEnumerable<Dog> Dogs = Dog.Repository.Values;
while ((searchText = GetInput()).Length > 0)
{
    foreach (var dog in ListDogsByNamePrefix(Dogs, searchText))
        Console.WriteLine(dog);
}


