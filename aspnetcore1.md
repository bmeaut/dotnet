## Default Builder Settings of Generic Host
https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-3.1#default-builder-settings

## Appsetting.json w DummySettings

```javascript
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*",
  "DummySettings": {
    "DefaultString": "My Value",
    "DefaultInt": 23,
    "SuperSecret": "Spoiler Alert!!!"
  }
}
```
## DummySettings class

```csharp
public class DummySettings
{
    public string DefaultString { get; set; }

    public int DefaultInt { get; set; }

    public string SuperSecret { get; set; }
}
```
## Get DummySettings injected into DummyController
```csharp
private DummySettings options;

public DummyController(IOptions<DummySettings> options)
{
    this.options=options.Value;
}
```

## DummySettings.Get getting data from IOption
```csharp
[HttpGet("{id}", Name = "Get")]
public string Get(int id)
{
    return id % 2 == 0 ? options.DefaultString : options.DefaultInt.ToString();
}
```


## secrets.json
```javascript
{
  "DummySettings": {
    "DefaultString": "My Value",
    "DefaultInt": 23,
    "SuperSecret": "SECRET"
  }
}
```
