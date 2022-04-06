# REST konvenciók
https://www.restapitutorial.com/lessons/httpmethods.html

# Scaffold parancs

```ps
dotnet aspnet-codegenerator controller -m WebApiLab.Dal.Entities.Product -dc WebApiLab.Dal.AppDbContext -outDir Controllers -name EFProductController -namespace WebApiLab.Api.Controllers -api
```

# IDesignTimeDbContextFactory, ha nem működne a scaffolding

Néha előforduló hiba, hogy nem működik a scaffolding. Erre egy megoldás, hogy felveszünk `IDesignTimeDbContextFactory<TContext>` interfészt megvalósító osztályrat. Ezt vegyünk fel `AppDbContextDesignTimeFactory` néven a DAL projektbe, az implementációban csak vissza kell adnunk egy DbContext példányt. Itt összerakunk egy config rendszert, amiből a connection stringet kiolvassuk.

```cs
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AcmeShopContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json", optional: false)
               .AddUserSecrets<AppDbContextFactory>()
               .AddEnvironmentVariables()
               .Build();

        var builder = new DbContextOptionsBuilder<AppDbContext>();
        builder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
        return new AppDbContext(builder.Options);
    }
}
```

# Product Post
## Header
Content-Type: application/json
## Body
```javascript
{
    "Name" : "Pálinka",
    "UnitPrice" : 4000,
    "ShipmentRegion" : 1,
    "CategoryId" : 1
}
```

# AutoMapper config
```csharp
public class WebApiProfile : Profile
{
    public WebApiProfile()
    {
        CreateMap<Dal.Entities.Product, Product>()
            .ForMember(
                p => p.Orders,
                opt => opt.MapFrom(x => x.ProductOrders.Select(po => po.Order)))
            .ReverseMap();
        CreateMap<Dal.Entities.Order, Order>().ReverseMap();
        CreateMap<Dal.Entities.Category, Category>().ReverseMap();
    }
}
```

# GetProduct by id XML comment

```csharp
/// <summary>
/// Get a specific product with the given identifier
/// </summary>
/// <param name="id">Product's identifier</param>
/// <returns>Returns a specific product with the given identifier</returns>
/// <response code="200">Returns a specific product with the given identifier</response>
```

# NSwag Studio

https://github.com/RicoSuter/NSwag/releases

A zip változat-ot töltsük le és csomagoljuk ki. Ezután NSwagStudio.exe-vel indítható az alkalmazás.

# Hellang.Middleware.ProblemDetails

```powershell
Install-Package Hellang.Middleware.ProblemDetails
```

# Product.RowVersion migráció

```powershell
Add-Migration ProductRowVersion -StartupProject WebApiLabor.Api
Update-Database -StartupProject WebApiLabor.Api
```

# ConcurrencyProblemDetails

```csharp
public class Conflict
{
    public object CurrentValue { get; set; }
    public object SentValue { get; set; }
}

public class ConcurrencyProblemDetails : StatusCodeProblemDetails
{
    public Dictionary<string, Conflict> Conflicts { get; }

    public ConcurrencyProblemDetails(DbUpdateConcurrencyException ex) :
        base(StatusCodes.Status409Conflict)
    {
        Conflicts = new Dictionary<string, Conflict>();
        var entry = ex.Entries[0];
        var props = entry.Properties
            .Where(p => !p.Metadata.IsConcurrencyToken).ToArray();
        var currentValues = props.ToDictionary(
            p => p.Metadata.Name, p => p.CurrentValue);

        //with DB values
        entry.Reload();

        foreach (var property in props)
        {
            if (!currentValues[property.Metadata.Name].
                Equals(property.CurrentValue))
            {
                Conflicts[property.Metadata.Name] = new Conflict
                {
                    CurrentValue = property.CurrentValue,
                    SentValue = currentValues[property.Metadata.Name]
                };
            }
        }
    }
}
```

# UpdateProduct fixek konkurenciakezeléshez

```csharp
public void UpdateProduct(int productId, Product updatedProduct)
{
    updatedProduct.Id = productId;
    var entry = _context.Attach(updatedProduct);
    entry.State = EntityState.Modified;            
    _context.SaveChanges();            
}

public async Task UpdateProductAsync(int productId, Product updatedProduct)
{
    updatedProduct.Id = productId;
    var entry = _context.Attach(updatedProduct);
    entry.State = EntityState.Modified;           
    await _context.SaveChangesAsync();
}
```
