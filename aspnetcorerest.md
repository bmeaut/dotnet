# REST konvenciók
https://www.restapitutorial.com/lessons/httpmethods.html

# AddDbContext

```cs
builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
```

# DbContext ctor

```cs
public AppDbContext(DbContextOptions<AppDbContext> options)
    : base(options) {}
```

# appsettings.Development.json

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Information",
    }
  },
  "ConnectionStrings": {
     "DefaultConnection": "<connection string>"
  }
}
```

# Scaffold parancs

```ps
dotnet tool install -g dotnet-aspnet-codegenerator

cd .\WebApiLab.Api

```

```ps
dotnet aspnet-codegenerator controller -m WebApiLab.Dal.Entities.Product -dc WebApiLab.Dal.AppDbContext -outDir Controllers -name EFProductController -namespace WebApiLab.Api.Controllers -api
```

# IDesignTimeDbContextFactory, ha nem működne a scaffolding

Néha előforduló hiba, hogy nem működik a scaffolding. Erre egy megoldás, hogy felveszünk `IDesignTimeDbContextFactory<TContext>` interfészt megvalósító osztályrat. Ezt vegyünk fel `AppDbContextDesignTimeFactory` néven a DAL projektbe, az implementációban csak vissza kell adnunk egy DbContext példányt. Conn stringnek elég ha valamilyen dummy értéket felveszünk. Viszont ezután a migrációk esetében a conn stringet paraméterként át kell adnink a CLI parancsnak.

```cs
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<AppDbContext>();
        builder.UseSqlServer("dummy");
        return new AppDbContext(builder.Options);
    }
}
```

# ProductService v1

```cs
public class ProductService : IProductService
{
    private readonly AppDbContext _context;

    public ProductService(AppDbContext context)
    {
        _context = context;
    }       

    public IEnumerable<Product> GetProducts()
    {
        var products = _context.Products
            .Include(p => p.Category)
            .Include(p => p.ProductOrders)
                .ThenInclude(po => po.Order)
            .ToList();

        return products;
    }
    /*Többi függvény generált implementációja*/
}
```
# AutoMapper profil

```cs
using AutoMapper;

namespace WebApiLab.Bll.Dtos;

public class WebApiProfile : Profile
{
    public WebApiProfile()
    {
        CreateMap<Dal.Entities.Product, Product>().ReverseMap();
        CreateMap<Dal.Entities.Order, Order>().ReverseMap();
        CreateMap<Dal.Entities.Category, Category>().ReverseMap();
    }
}
```

# ProductService w mapping

```cs
public IEnumerable<Product> GetProducts()
{
    var products = _context.Products
        .ProjectTo<Product>(_mapper.ConfigurationProvider)
        .AsEnumerable();
    return products;
}
```

```cs
public Product GetProduct(int productId)
{
    return _context.Products
        .ProjectTo<Product>(_mapper.ConfigurationProvider)
        .SingleOrDefault(p => p.Id == productId)
        ?? throw new EntityNotFoundException("Nem található a termék");
}
```

```cs
public Product InsertProduct(Product newProduct)
{
    var efProduct = _mapper.Map<Dal.Entities.Product>(newProduct);
    _context.Products.Add(efProduct);
    _context.SaveChanges();
    return GetProduct(efProduct.Id);
}
```

```cs
public void UpdateProduct(int productId, Product updatedProduct)
{
    var efProduct = _mapper.Map<Dal.Entities.Product>(updatedProduct);
    efProduct.Id = productId;
    _context.Entry(efProduct).State = EntityState.Modified;
    _context.SaveChanges();
}
```

```cs
public void DeleteProduct(int productId)
{
    _context.Products.Remove(new Dal.Entities.Product(null!) { Id = productId });
    _context.SaveChanges();
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
