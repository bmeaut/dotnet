# Init

https://github.com/bmeaut/WebApiLab/archive/refs/heads/net6-test-init.zip

# Nuget

```xml
<PackageReference Include="Bogus" Version="34.0.2" />
<PackageReference Include="FluentAssertions" Version="6.6.0" />
<PackageReference Include="Microsoft.AspNetCore.Mvc.Testing" Version="6.0.4" />
```

# Project reference

```xml
<ItemGroup>
  <ProjectReference Include="..\WebApiLab.Api\WebApiLab.Api.csproj" />
</ItemGroup>
```

# CustomWebApplicationFactory

```csharp
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment("Development");
        builder.ConfigureServices(services =>
        {
            services.AddScoped(sp =>
            {
                return new DbContextOptionsBuilder<AppDbContext>()
                    .UseSqlServer(@"connection string")
                    .UseApplicationServiceProvider(sp)
                    .Options;
            });
        });

        var host = base.CreateHost(builder);

        using var scope = host.Services.CreateScope();
        scope.ServiceProvider.GetRequiredService<AppDbContext>()
            .Database.EnsureCreated();

        return host;
    }
}
```

# ProductControllerTest

```csharp
public partial class ProductControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _appFactory;

    public ProductControllerTests(CustomWebApplicationFactory appFactory)
    {
        _appFactory = appFactory;
    }
}
```

# DTO fake

```csharp
private readonly Faker<Product> _dtoFaker;

public ProductControllerTests(CustomWebApplicationFactory appFactory)
{
    _dtoFaker = new Faker<Product>()
        .RuleFor(p => p.Id, 0)
        .RuleFor(p => p.Name, f => f.Commerce.Product())
        .RuleFor(p => p.UnitPrice, f => f.Random.Int(200, 20000))
        .RuleFor(p => p.ShipmentRegion, 
                 f => f.PickRandom<Dal.Entities.ShipmentRegion>())
        .RuleFor(p => p.CategoryId, 1)
        .RuleFor(p => p.RowVersion, f => f.Random.Bytes(5));
}
```

# JsonSerializerOptions

```csharp
private readonly JsonSerializerOptions _serializerOptions;

public ProductControllerTests(CustomWebApplicationFactory appFactory)
{
    // ...
    _serializerOptions = new JsonSerializerOptions()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
    _serializerOptions.Converters.Add(new JsonStringEnumConverter());
}
```

# ProductsControllerTest

```csharp
public partial class ProductControllerTests
{
    public class Post : ProductControllerTests
    {
        public Post(CustomWebApplicationFactory appFactory)
            : base(appFactory)
        {
        }
    }
}
```

# Test method #1

## v0

```csharp
[Fact]
public async Task Should_Succeded_With_Created()
{
    // Arrange

    // Act

    // Assert
}
```

## Arrange

```csharp
// Arrange
var client = _appFactory.CreateClient();
var dto = _dtoFaker.Generate();
```

# Act

```csharp
// Act
var response = await client.PostAsJsonAsync("/api/product", dto, _serializerOptions);
var p = await response.Content.ReadFromJsonAsync<Product>(_serializerOptions);
```

# Assert

```csharp
// Assert
response.StatusCode.Should().Be(HttpStatusCode.Created);
response.Headers.Location
    .Should().Be(
        new Uri(_appFactory.Server.BaseAddress, $"/api/Product/{p.Id}")
    );

p.Should().BeEquivalentTo(
    dto,
    opt => opt.Excluding(x => x.Category)
        .Excluding(x => x.Orders)
        .Excluding(x => x.Id)
        .Excluding(x => x.RowVersion));
p.Category.Should().NotBeNull();
p.Category.Id.Should().Be(dto.CategoryId);
p.Orders.Should().BeEmpty();
p.Id.Should().BeGreaterThan(0);
p.RowVersion.Should().NotBeEmpty();
```

## prevent mutation

```csharp
// Arrange
_appFactory.Server.PreserveExecutionContext = true;
using var tran = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
```

# Test method #2

## v0

```csharp
[Theory]
[InlineData("", "Product name is required.")]
[InlineData(null, "Product name is required.")]
public async Task Should_Fail_When_Name_Is_Invalid(string name, string expectedError)
{
    // Arrange

    // Act

    // Assert
}
```

## Arrange

```csharp
// Arrange
var client = _appFactory.CreateClient();
var dto = _dtoFaker.RuleFor(x => x.Name, name).Generate();
```

# Act

```csharp
var response = await client.PostAsJsonAsync("/api/product", dto, _serializerOptions);
var p = await response.Content
            .ReadFromJsonAsync<ValidationProblemDetails>(_serializerOptions);
```

## Assert

```csharp
// Assert
response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

p.Status.Should().Be(400);
p.Errors.Should().HaveCount(1);
p.Errors.Should().ContainKey(nameof(Product.Name));
p.Errors[nameof(Product.Name)].Should().ContainSingle(expectedError);
```
