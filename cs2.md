## Dog class
```csharp
public class Dog
{
    public string Name { get; set; }
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime DateOfBirth { get; set; }
    private int AgeInDays => DateTime.Now.Subtract(DateOfBirth).Days;
    public int Age => AgeInDays / 365;
    public int AgeInDogYears => AgeInDays * 7 / 365;
    public override string ToString() =>
        $"{Name} ({Age} | {AgeInDogYears}) [ID: {Id}]";
}
```

## Dictionary
```csharp
public Dictionary<string, object> Metadata { get; } = new ();
public object this[string key]
{
    get { return Metadata[key]; }
    set { Metadata[key] = value; }
}
```

## Indexer
```csharp
var pimpedli = new Dog
{
    Name = "Pimpedli",
    DateOfBirth = new DateTime(2006, 06, 10),
        ["Chip azonosító"] = "123125AJ"
};
```

```csharp
var dogs = new Dictionary<string, Dog>
{
    ["banan"] = banan,
    ["watson"] = watson,
    ["unnamed"] = unnamed,
    ["unknown"] = unknown,
    ["pinmpedli"] = pimpedli
};

foreach (var dog in dogs)
    Console.WriteLine($"{dog.Key} - {dog.Value}");
```

```csharp
// szándékosan hibás
var dogs = new Dictionary<string, Dog>
{
    [banan.Name] = banan,
    [watson.Name] = watson,
    [unnamed.Name] = unnamed,
    [unknown.Name] = unknown,
    [pimpedli.Name] = pimpedli
};
//ArgumentNullException!
```

```csharp
var dogs = new Dictionary<string, Dog>
{
    [nameof(banan)] = banan,
    [nameof(watson)] = watson,
    [nameof(unnamed)] = unnamed,
    [nameof(unknown)] = unknown,
    [nameof(pimpedli)] = pimpedli
};
```

```csharp
var dogs = new Dictionary<string, Dog>
{
    { nameof(banan), banan},
    { nameof(watson), watson},
    { nameof(unnamed), unnamed},
    { nameof(unknown), unknown},
    { nameof(pimpedli), pimpedli}
};
```

## AgeInDays w Elvis/Kozsó operator
```csharp
private int? AgeInDays => (-DateOfBirth?.Subtract(DateTime.Now))?.Days;
```

## Rekord
```csharp
public record class DogRec(
    Guid Id,
    string Name,
    DateTime? DateOfBirth=null,
    Dictionary<string, object> Metadata=null
);
```
