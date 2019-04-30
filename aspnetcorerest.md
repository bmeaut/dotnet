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

