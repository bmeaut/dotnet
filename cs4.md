## RandomOrderedList<> 
```
public class RandomOrderedList<T> : IEnumerable<T>
{
    public RandomOrderedList(IList<T> source) => Source = source;
    private IList<T> Source { get; }
    public IEnumerator<T> GetEnumerator() => new RandomEnumerator<T>(Source);
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
```

## RandomEnumerator
```
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
