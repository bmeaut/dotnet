# Category
```csharp
public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<Product> Products { get; } = new List<Product>();
}
```

# Order
```csharp
public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }

    public ICollection<ProductOrder> ProductOrders { get; } = new List<ProductOrder>();
}
```

# Product
```csharp
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int UnitPrice { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; }

    public ICollection<ProductOrder> ProductOrders { get; } = new List<ProductOrder>();
}
```

# ProductOrder
```csharp
public class ProductOrder
{
    public int Id { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; }

    public int OrderId { get; set; }
    public Order Order { get; set; }
}
```
# GetLoggerFactory
```csharp
private ILoggerFactory GetLoggerFactory()
{
  IServiceCollection serviceCollection = new ServiceCollection();
  serviceCollection.AddLogging(builder => builder.AddConsole());
  return serviceCollection.BuildServiceProvider()
          .GetService<ILoggerFactory>();
}
```
# SeedDatabase v2 - HIBÁS!
```csharp
//kóddarabka
static void SeedDatabase(NorthwindContext ctx)
{
    var cat_drink = new Category { Name = "Ital2" };
    var sör2 = new Product { Name = "Sör2", UnitPrice = 50, CategoryId = cat_drink.Id };
    ctx.Products.Add(sör2);
    ctx.Categories.Add(cat_drink);
    ctx.SaveChanges();
}
```

# SeedDatabase v3
```csharp
//kóddarabka
static void SeedDatabase(NorthwindContext ctx)
{
    var cat_drink = new Category { Name = "Ital2" };
    var sör2 = new Product { Name = "Sör2", UnitPrice = 50, Category = cat_drink };
    ctx.Products.Add(sör2);
    ctx.Categories.Add(cat_drink);
    ctx.SaveChanges();
}
```

# Seeding products w HasData

```csharp
modelBuilder.Entity<Product>()
    .HasData(new Product { Id=1, Name = "Sör", UnitPrice = 50, CategoryId = 1 },
             new Product { Id=2, Name = "Bor", UnitPrice = 550, CategoryId = 1 },
             new Product { Id=3, Name = "Tej", UnitPrice = 260, CategoryId = 1 });
```

# Seeding Order

```csharp
modelBuilder.Entity<Order>().HasData(
     new Order {Id = 1, OrderDate = new DateTime(2019, 02, 01)}
);

modelBuilder.Entity<ProductOrder>().HasData(
    new ProductOrder { Id = 1, OrderId = 1, ProductId = 1},
    new ProductOrder { Id = 2, OrderId = 1, ProductId = 2 }
);
```

# ShipmentRegion

```csharp
[Flags]
public enum ShipmentRegion
{
    EU = 1,
    NorthAmerica = 2,
    Asia = 4,
    Australia = 8
}
```

# Seeding more products w HasData
```csharp
modelBuilder.Entity<Product>().HasData(
    new Product
    {
         Id =1, Name = "Sör", UnitPrice = 50, CategoryId = 1,
         ShipmentRegion = ShipmentRegion.Asia
    },
    new Product { Id=2, Name = "Bor", UnitPrice = 550, CategoryId = 1 },
    new Product { Id=3, Name = "Tej", UnitPrice = 260, CategoryId = 1 },
    new Product
    {
        Id = 4, Name = "Whiskey", UnitPrice = 960, CategoryId = 1,
        ShipmentRegion = ShipmentRegion.Australia
    },
    new Product
    {
        Id = 5, Name = "Rum", UnitPrice = 850, CategoryId = 1,
        ShipmentRegion = ShipmentRegion.EU | ShipmentRegion.NorthAmerica
    }
  );
```
