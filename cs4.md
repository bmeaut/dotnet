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
        return result/elements;
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


## RandomOrderedList<> 
```csharp
public class RandomOrderedList<T> : IEnumerable<T>
{
    public RandomOrderedList(IList<T> source) => Source = source;
    private IList<T> Source { get; }
    public IEnumerator<T> GetEnumerator() => new RandomEnumerator<T>(Source);
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
```

## RandomEnumerator
```csharp
public class RandomEnumerator<T> : IEnumerator<T>
{
    private IList<T> Source { get; }
    private int[] indexes;
    private int currentIndex;
    public RandomEnumerator(IList<T> source)
    {
        Source = source;
        Reset();
    }
    public T Current => Source[indexes[currentIndex]];
    object IEnumerator.Current => Current;
    public void Dispose() { }
    public bool MoveNext() => ++currentIndex < Source.Count;
    public void Reset() => indexes = Enumerable.Range(currentIndex = 0, Source.Count)
                                               .OrderBy(i => Guid.NewGuid()).ToArray();
}
```
