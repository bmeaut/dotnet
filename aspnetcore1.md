## Default Builder Settings of Web Host
 https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/web-host

## Appsetting.json
```javascript
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft.AspNetCore": "Debug"
    }
  }
}
```

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
 "AllowedHosts": "*", // a sor végére bekerült egy vessző
 "DummySettings": {
   "DefaultString": "My Value",
   "DefaultInt": 23,
   "SuperSecret":  "Spoiler Alert!!!"
 }
}
```

## DummySettings class

```csharp
public class DummySettings
{
    public string? DefaultString { get; set; }

    public int DefaultInt { get; set; }

    public string? SuperSecret { get; set; }
}
```


## DummySettings config

```csharp
    builder.Services.Configure<DummySettings>(
        builder.Configuration.GetSection(nameof(DummySettings)));
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
    return id % 2 == 0 ? (options.DefaultString ?? "value") : options.DefaultInt.ToString();
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
