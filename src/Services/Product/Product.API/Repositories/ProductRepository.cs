using Contracts.Common.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Product.API.Entities;
using Product.API.Presistence;
using Product.API.Repositories.Interfaces;

namespace Product.API.Repositories;

public class ProductRepository : RepositoryBaseAsync<CatalogProduct, long, ProductContext>, IProductRepository
{
    public ProductRepository(ProductContext context, IUnitOfWork<ProductContext> unitOfWork) : base(context, unitOfWork)
    {

    }
    public async Task CreateProduct(CatalogProduct product)
        => await CreateProduct(product);

    public async Task DeleteProduct(long Id)
    {
        var product = await GetProductById(Id);
        if (product != null) await DeleteAsync(product);
    }

    public async Task<CatalogProduct> GetProductById(long Id)
        => await GetByIdAsync(Id);

    public async Task<IEnumerable<CatalogProduct>> GetProducts()
        => await FindAll().ToListAsync();

    public async Task UpdateProduct(CatalogProduct product)
        => await UpdateAsync(product);
}