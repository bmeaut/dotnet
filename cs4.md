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
