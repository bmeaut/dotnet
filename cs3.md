## ListDogs - Predicate

```csharp
static List<Dog> ListDogsByPredicate(IEnumerable<Dog> dogs,
                                             Predicate<Dog> predicate)
{
    var result = new List<Dog>();
    foreach (var dog in dogs)
    {
        if (predicate(dog))
            result.Add(dog);
    }
    return result;
}
```

```csharp
foreach(var dog in ListDogsByPredicate(Dogs,
    delegate (Dog d) {
        return d.Name.StartsWith(searchText,
                                StringComparison.OrdinalIgnoreCase);
                    })
        )
```

## Enumerable operátorok

```csharp
namespace HelloLinq.Extensions.Enumerable;

public static class EnumerableExtensions
{
    public static int Sum<T> (IEnumerable<T>  source,
                                  Func<T, int>  sumSelector)
    {
        var result = 0;
        foreach (var elem in source)
            result += sumSelector(elem);
        return result;
    }
}
```

```csharp
namespace HelloLinq.Extensions.Enumerable;

Console.WriteLine("Életkorok összege: " +
    $"{EnumerableExtensions.Sum(Dogs, d => d.Age ?? 0)}");
```

```csharp
public static double Average<T> (this IEnumerable<T>  source,
                                             Func<T, int>  sumSelector)
        {
            var result = 0.0; // Az osztás művelet miatt double
            var elements = 0;
            foreach (var elem in source)
            {
                elements++;
                result += sumSelector(elem);
            }
            return result/elements;
        }
        public static int Min<T> (this IEnumerable<T>  source,
                                      Func<T, int>  valueSelector)
        {
            int value = int.MaxValue;
            foreach (var elem in source)
            {
                var currentValue = valueSelector(elem);
                if (currentValue < value)
                    value = currentValue;
            }
            return value;
        }
        public static int Max<T> (this IEnumerable<T>  source,
                                      Func<T, int>  valueSelector)
            => -source.Min(e => -valueSelector(e));
```

```csharp
Console.WriteLine($"Átlagos életkor: {Dogs.Average(d => d.Age ?? 0)}");
Console.WriteLine(
     $"Minimum-maximum életkor: " + 
     $"{Dogs.Min(d => d.Age ?? 0)} | {Dogs.Max(d => d.Age ?? 0)}");
```

```csharp
public static IEnumerable<T>  
            Where<T> (this IEnumerable<T>  source,
                           Predicate<T>  predicate)
{
    foreach (var elem in source)
    {
        if (predicate(elem))
            yield return elem;
    }
}
public static IEnumerable<TValue> 
        Select<T, TValue>(this IEnumerable<T>  source,
                               Func<T, TValue> selector)
{
    foreach (var elem in source)
    {
        yield return selector(elem);
    }
}
```

```csharp
foreach (var text in Dogs
    .Where(d => d.DateOfBirth?.Year < 2010)
    .Select(d => $"{d.Name} ({d.Age}))"))
{
    Console.WriteLine(text);
}
```

```csharp
var query = from d in Dogs
            where d.DateOfBirth?.Year < 2010
            select new
            {
                Dog = d,
                AverageSiblingAge = d.Siblings.Average(s => s.Age ?? 0)
            };
int maxLength = query.Max(d => d.Dog.Name.Length);
foreach (var meta in query)
{
    Console.WriteLine(
        $"{meta.Dog.Name.TrimPad(maxLength)} - {meta.AverageSiblingAge.TrimPad(5)}");
}
```

## Expression építés Expression API-val
```csharp
using Expression = System.Linq.Expressions.Expression;
using System.Reflection; // A GetTypeInfo() bővítő metódus miatt.
//...
var param =              Expression.Parameter(typeof(Dog), "d");
var name =               Expression.Property(param, "Name");
var startsWithConstant = Expression.Constant(searchText);
var startsWithArgument = Expression.Constant(StringComparison.OrdinalIgnoreCase);
var methodCall =         Expression.Call(name, typeof(string).GetTypeInfo().
                                GetMethod("StartsWith",
                                    new[] { typeof(string),
                                    typeof(StringComparison) }),
                            startsWithConstant, startsWithArgument);
var expression =         Expression.Lambda(methodCall, param);
var predicate =          new Predicate<Dog>(expression.Compile() as Func<Dog, bool>);
```
