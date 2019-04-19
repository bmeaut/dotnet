# REST konvenciók
https://www.restapitutorial.com/lessons/httpmethods.html

# Product Post
## Header
Content-Type: application/json
## Body
```
{
    "Name" : "Pálinka",
    "UnitPrice" : 4000,
    "ShipmentRegion" : 1,
    "CategoryId" : 1
}
```

# AutoMapper config
```
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
