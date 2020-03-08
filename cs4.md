## Non-nullable ref types
Source: https://devblogs.microsoft.com/dotnet/nullable-reference-types-in-csharp/

### Phase 1 - base type
```csharp
class Person
{
    string FirstName;   // Not null
    string? MiddleName; // May be null
    string LastName;    // Not null
}
```

### Phase 2 - add constructor
```csharp
public Person(string fname, string lname, string? mname)
{
    FirstName = fname;
    LastName = lname;
    MiddleName = mname;
}
```

### Phase 3 - null value detection examples
```csharp
void M(string? ns)            // ns is nullable
{
    WriteLine(ns.Length);     // WARNING: may be null
    if (ns != null)
    {
        WriteLine(ns.Length); // ok, not null here 
    }
    if (ns == null)
    {
        return;               // not null after this
    }
    WriteLine(ns.Length);     // ok, not null here
    ns = null;                // null again!
    WriteLine(ns.Length);     // WARNING: may be null
}
```

### Phase 4 - sneaky functions

```csharp
private Person GetAnotherPerson()
{
    return new Person(LastName, FirstName, MiddleName ?? string.Empty);
}

private void ResetFields()
{
    FirstName = default!;
    LastName = null!;
    MiddleName = null;
}
```

### Phase 5 - testing w sneaky funcions

```csharp
void M(Person p)
{
    if (p.MiddleName != null)
    {
        p.ResetFields();             // can't detect change
        WriteLine(p.MiddleName.Length); // ok 

        p = GetAnotherPerson();         // that's too obvious
        WriteLine(p.MiddleName.Length); // WARNING: saw that!
    }
    p.FirstName = null;          // 1 WARNING: it's null
    p.LastName = p.MiddleName;   // 2 WARNING: may be null
    string s = default(string);  // 3 WARNING: it's null

}
```

### Phase 6 - sneaky struct field
```csharp
struct PersonHandle
{
    public Person person;        // 5 ok: too common
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
