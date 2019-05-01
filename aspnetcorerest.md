# REST konvenciók
https://www.restapitutorial.com/lessons/httpmethods.html

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
services.AddAutoMapper(cfg =>
{
    cfg.CreateMap<Entities.Product, Dtos.Product>()
        .ForMember(dto => dto.Orders, opt => opt.Ignore())
        .AfterMap((p, dto, ctx) =>
            dto.Orders = p.ProductOrders.Select(po =>
            ctx.Mapper.Map<Dtos.Order>(po.Order)).ToList()).ReverseMap();
    cfg.CreateMap<Entities.Order, Dtos.Order>().ReverseMap();
    cfg.CreateMap<Entities.Category, Dtos.Category>().ReverseMap();
});
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

# UpdateProduct fixek

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
