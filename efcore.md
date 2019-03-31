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
