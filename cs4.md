## Kollekció bejárás

```csharp
var numbers = Enumerable.Range(1, 8).ToList();
foreach (var p in numbers)
{
    if (p % 2 == 0)
    {
        numbers.Remove(p);
    }
}
numbers.ForEach(Console.WriteLine);
```

```csharp
var i = 0;
foreach (var n in numbers
                    .Where(p => p > 2)
                    .Select(p => new { p, x = ++i }))
{
    Console.WriteLine($"{n} - {i}");
}

Console.WriteLine();

i = 0;
foreach (var n in numbers
                    .Where(p => p > 2)
                    .Select(p => new { p, x = ++i })
                    .ToList())
{
    Console.WriteLine($"{n} - {i}");
}
```

## Async-await

```csharp
LoadWebPageAsync();
Console.WriteLine("Ez a vége");
Console.ReadKey();

static async void LoadWebPageAsync()
{
    using (var client = new HttpClient())
    {
        var response = await client.GetAsync(new Uri("http://www.bing.com"));
        Console.WriteLine(response.StatusCode.ToString());

        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine(content.Take(1000).ToArray());
    }
}
```

```csharp
var content = await LoadWebPageAsync();
Console.WriteLine(content);

Console.WriteLine("Ez a vége");
Console.ReadKey();

static async Task<string> LoadWebPageAsync() //generikus paraméter
{
    using (var client = new HttpClient())
    {
        var response = await client.GetAsync(new Uri("http://www.bing.com"));
        Console.WriteLine(response.StatusCode.ToString());

        var content = await response.Content.ReadAsStringAsync();
        return new string(content.Take(1000).ToArray());
    }
}
```

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

## Tuple, lokális függvények, Dispose

```csharp
(long Current, long Previous) Fib(long i)
{
    if (i == 0) return (1, 0);
    var (curr, prev) = Fib(i - 1);
    Thread.Sleep(100); //<3>
    return (curr + prev, curr);
}

return x < 0
    ? throw new ArgumentException("Less negativity please!", nameof(x))
    : Fib(x).Current;
```

```csharp
var sw = Stopwatch.StartNew();
foreach (var n in Enumerable.Range(1, 15))
{
  Console.WriteLine($"{n}: {Fibonacci(n)}");
}
sw.Stop();
Console.WriteLine($"Elapsed: {sw.ElapsedMilliseconds}");
Console.ReadKey()
```

```csharp
public class StopwatchWrapper : IDisposable
{
    public Stopwatch Stopwatch { get; }

    public string Title { get; }

    public StopwatchWrapper(string? title = default)
    {
        Title = title ?? Guid.NewGuid().ToString();
        Console.WriteLine($"Task {title} starting at {DateTime.Now}.");
        Stopwatch = Stopwatch.StartNew();
    }
}
```

```csharp
using (new StopwatchWrapper("Fib 1-15"))
{
    foreach (var n in Enumerable.Range(1, 15))
    {
        Console.WriteLine($"{n}: {Fibonacci(n)}");
    }
} 
```

```csharp
public void Snapshot(string text) =>
    Console.WriteLine(
        $"Task {Title} snapshot {text}: {Stopwatch.ElapsedMilliseconds} ms"
    );
```
