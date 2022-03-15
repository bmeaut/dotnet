# Category
```csharp
public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Product> Products { get; } 
                                = new List<Product>();
    public Category(string name)
    {
        Name = name;
    }
}
```

# Order
```csharp
public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public ICollection<OrderItem> OrderItems { get; } 
                                    = new List<OrderItem>();
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
    public Category Category { get; set; } = null!;
    public ICollection<OrderItem> ProductOrders { get; } 
                                        = new List<OrderItem>();
    public Product(string name)
    {
        Name = name;
    }     
}
```

# OrderItem
```csharp
public class OrderItem
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
    public int Quantity { get; set; }
}
```

# DbContext

```csharp
public class NorthwindContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"<connstring>");
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Order> Orders => Set<Order>();
}
```

# Migráció

```csharp
Add-Migration CategoryName_ProductName
Update-Database CategoryName_ProductName
```

# OnModelCreating

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<Category>()
        .Property(c => c.Name)
        .HasMaxLength(15)
        .IsRequired();
}
```

# Napózás
```csharp
 optionsBuilder.UseSqlServer(@"<connstring>")
            .LogTo(Console.WriteLine, LogLevel.Information);
```

# Seed v1

```csharp
static void SeedDatabase(NorthwindContext ctx)
{
    if (!ctx.Products.Any())
    {
        var cat_drink = new Category("Ital");
        var cat_food = new Category("Étel");
        ctx.Categories.Add(cat_drink);
        ctx.Categories.Add(cat_food);
        ctx.Products.Add(new Product("Sör")
            { UnitPrice = 50, Category = cat_drink });
        ctx.Products.Add(new Product("Bor")
            { Name = "Bor", Category = cat_drink });
        ctx.Products.Add(new Product("Tej")
            { Name = "Tej", CategoryId = cat_drink.Id });
        ctx.SaveChanges();
    }
}
```

# Seed w HasData

```csharp
modelBuilder.Entity<Category>().HasData(
    new Category ("Ital") { Id = 1 }
);

modelBuilder.Entity<Product>().HasData(
    new Product("Sör") { Id = 1, UnitPrice = 50, CategoryId = 1 },
    new Product("Bor") { Id = 2, UnitPrice = 550, CategoryId = 1 },
    new Product("Tej") { Id = 3, UnitPrice = 260, CategoryId = 1 }
);
```

# Query v1

```csharp
var q = from p in ctx.Products
        where p.Name.Contains("ö")
        select p.Name;

foreach (var name in q)
{
    Console.WriteLine(name);
}
```

# Query stitching

```csharp
var q = from p in ctx.Products.TagWith("Névszűrés")
        where p.Name.Contains("r")
        select p;

var q2 = from p in q
         where p.UnitPrice > 20
         select p.Name;

foreach (var name in q2)
{
    Console.WriteLine(name);
}
```

# Insert Order

```csharp
var q = from p in ctx.Products
        where p.Name.Contains("r")
        select p;

var order = new Order { OrderDate = DateTime.Now };
foreach (var p in q)
{
    order.OrderItems.Add(
        new OrderItem { Product = p, Order = order, Quantity=2 }
    );
}

ctx.Orders.Add(order);
ctx.SaveChanges();
```

# Related data v1

```csharp
var products = ctx.Products;

foreach (Product p in products)
{
    Console.WriteLine($"{p.Name} ({p.Category.Name})");
}
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
