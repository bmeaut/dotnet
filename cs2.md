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

    public override string ToString() => $"{Name} ({Age} | {AgeInDogYears}) [ID: {Id}]";
}
```

## AgeInDays w Elvis/Kozsó operator
```csharp
private int? AgeInDays => (-DateOfBirth?.Subtract(DateTime.Now))?.Days;
```

## Egysoros TrimPad
```csharp
public static string TrimPad(string text, int length) =>
    ((text?.Length ?? 0) == 0)
        ? new string(' ', length)
        : (text.Length <= length)
            ? text + new string(' ', length - text.Length)
            : text.Substring(0, length);
```
