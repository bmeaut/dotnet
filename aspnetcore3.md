## WriteAsJsonAsync
```csharp
private async Task WriteAsJsonAsync(
    HttpContext context, HttpStatusCode statusCode, object payload, bool clearBeforeWrite = true)
{
    if (clearBeforeWrite)
    {
        context.Response.Clear();
    }

    context.Response.StatusCode = (int)statusCode;

    context.Response.ContentType = "application/json";
    var json = JsonConvert.SerializeObject(payload);
    await context.Response.WriteAsync(json);
}
```
## UseApiExceptionHandler

```csharp
public static class ApiExceptionHandlerExtensions
{
    public static IApplicationBuilder UseApiExceptionHandler(this IApplicationBuilder app)
    {
        return app.UseWhen(
            ctx => ctx.Request.Path.HasValue 
                   && ctx.Request.Path.StartsWithSegments(new PathString("/api")),
            b => b.UseMiddleware<ApiExceptionHandlerMiddleware>());
    }
}
```

## product service

``` C# 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using WebApiLabor.Bll.Context;
using WebApiLabor.Bll.Entities;
using WebApiLabor.Bll.Exceptions;

namespace WebApiLabor.Bll.Services
{
    public class ProductService : IProductService
    {
        private readonly NorthwindContext _context;

        public ProductService(NorthwindContext context)
        {
            _context = context;
        }

        public Product GetProduct(int productId)
        {
            return _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductOrders)
                    .ThenInclude(po => po.Order)
                .SingleOrDefault(p => p.Id == productId) 
                ?? throw new EntityNotFoundException("Nem található a termék");
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

        public Product InsertProduct(Product newProduct)
        {
            _context.Products.Add(newProduct);

            _context.SaveChanges();

            return newProduct;
        }

        public void UpdateProduct(int productId, Product updatedProduct)
        {
            updatedProduct.Id = productId;
            var entry = _context.Attach(updatedProduct);
            entry.State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException e)
            {
                // Létezett-e egyáltalán
                var _ = _context.Products.AsNoTracking().SingleOrDefault(p => p.Id == productId) ?? throw new EntityNotFoundException("Nem található a termék");

                throw;
            }
        }

        public void DeleteProduct(int productId)
        {
            _context.Products.Remove(new Product { Id = productId });

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException e)
            {
                // Létezett-e egyáltalán
                var _ = _context.Products.AsNoTracking().SingleOrDefault(p => p.Id == productId) ?? throw new EntityNotFoundException("Nem található a termék");

                throw;
            }
        }
    }
}

```
