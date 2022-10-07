using Microsoft.EntityFrameworkCore;
using Product.API.Entities;

namespace Product.API.Presistence;

public class ProductContext : DbContext
{
    public ProductContext(DbContextOptions<ProductContext> options) : base(options)
    {

    }
    public DbSet<CatalogProduct> Products { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(cancellationToken);
    }
}