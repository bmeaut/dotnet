## Azonnali kiértékelésű operátorok
```csharp
public static class EnumerableExtensions
{
    public static int Sum<T>(this IEnumerable<T> source, Func<T, int> sumSelector)
    {
        var result = 0;
        foreach (var elem in source)
            result += sumSelector(elem);
        return result;
    }

    public static double Average<T>(this IEnumerable<T> source, Func<T, int> sumSelector)
    {
        var result = 0.0; // Az osztás művelet miatt double
        var elements = 0;
        foreach (var elem in source)
        {
            elements++;
            result += sumSelector(elem);
        }
        return result / elements;
    }

    public static int Min<T>(this IEnumerable<T> source, Func<T, int> valueSelector)
    {
        int value = 0;
        foreach (var elem in source)
        {
            var currentValue = valueSelector(elem);
            if (currentValue < value || value == null)
                value = currentValue;
        }
        return value;
    }

    public static int Max<T>(this IEnumerable<T> source, Func<T, int> valueSelector)
        => -source.Min(e => -valueSelector(e));
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
