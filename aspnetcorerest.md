# Kiinduló projekt

- https://github.com/bmeaut/WebApiLab
- https://github.com/bmeaut/WebApiLab/archive/refs/heads/master.zip

# NuGet csomagok

- Microsoft.EntityFrameworkCore.Tools
- Microsoft.VisualStudio.Web.CodeGeneration.Design
- Hellang.Middleware.ProblemDetails

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

# ProblemDetails in catch

```csharp
ProblemDetails details= new ProblemDetails
{
    Title = "Invalid ID",
    Status = StatusCodes.Status404NotFound,
    Detail = $"No product with ID {id}"
};
return NotFound(details); //ProblemDetails átadása
```

# ProblemDetails by MW

```csharp
builder.Services.AddProblemDetails(options =>
{
    options.IncludeExceptionDetails = (ctx,ex) => false;
    options.MapToStatusCode<EntityNotFoundException>(StatusCodes.Status404NotFound);
});
```

# UpdateProduct Exception => ProblemDetails

```csharp
try
{
     _context.SaveChanges();
}
catch (DbUpdateConcurrencyException)
{
    if (_context.Products.SingleOrDefault(p => p.Id == productId) == null)
        throw new EntityNotFoundException("Nem található a termék");
    else
        throw;
}
```

# DeleteProduct Exception => ProblemDetails

```csharp
try
{
    _context.SaveChanges();
}
catch (DbUpdateConcurrencyException)
{
    if (_context.Products.SingleOrDefault(p => p.Id == productId) == null)
        throw new EntityNotFoundException("Nem található a termék");
    else
        throw;
}
```

# Custom mapping Exception => ProblemDetails

```csharp
services.AddProblemDetails(options =>
{
    options.IncludeExceptionDetails = (ctx, ex) => false;
    options.Map<EntityNotFoundException>(
        (ctx, ex) =>
        {
            var pd=StatusCodeProblemDetails.Create(StatusCodes.Status404NotFound);
            pd.Title = ex.Message;
            return pd;
        }
    );
});
```

# NET 6 client

```csharp
Console.Write("ProductId: ");
var id = Console.ReadLine();
if(id != null)
    await GetProductAsync(int.Parse(id));

Console.ReadKey();

static async Task GetProductAsync(int id)
{
    using var client = new HttpClient();

    /*A portot írjuk át a szervernek megfelelően*/
    var response = await client.GetAsync(new Uri($"http://localhost:5184/api/Product/{id}"));
    response.EnsureSuccessStatusCode();
    var jsonStream = await response.Content.ReadAsStreamAsync();
    var json = await JsonDocument.ParseAsync(jsonStream);
    Console.WriteLine($"{json.RootElement.GetProperty("name")}:" +
        $"{json.RootElement.GetProperty("unitPrice")}.-");
}
```


# GetProduct by id XML comment

```csharp
/// <summary>
/// Get a specific product with the given identifier
/// </summary>
/// <param name="id">Product's identifier</param>
/// <returns>Returns a specific product with the given identifier</returns>
/// <response code="200">Listing successful</response>
```


# PostProduct by id XML comment

```csharp
/// <summary>
/// Creates a new product
/// </summary>
/// <param name="product">The product to create</param>
/// <returns>Returns the product inserted</returns>
/// <response code="201">Insert successful</response>
```

# Enum as string

```csharp
.AddJsonOptions(opts =>
{
    //opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
```

# NSwag Studio

https://github.com/RicoSuter/NSwag/releases

A zip változat-ot töltsük le és csomagoljuk ki. Ezután NSwagStudio.exe-vel indítható az alkalmazás.

# GetProducts2Async

```csharp
/**/if (id != null)
    {
        //await GetProductAsync(int.Parse(id));
        var p = await GetProduct2Async(int.Parse(id));
        Console.WriteLine($"{p.Name}: {p.UnitPrice}");
    }

static async Task<Product> GetProduct2Async(int id)
{
    using var httpClient = new HttpClient() 
        { BaseAddress = new Uri("http://localhost:5000/") };
    var client = new ProductClient(httpClient);
    return await client.GetAsync(id);
}
```
# RowVersion model config

```csharp
modelBuilder.Entity<Product>()
    .Property(p => p.RowVersion)
    .IsRowVersion();
```

# Product.RowVersion migráció

```powershell
Add-Migration ProductRowVersion
Update-Database
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
