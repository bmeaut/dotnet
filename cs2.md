## Dog class
```
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
