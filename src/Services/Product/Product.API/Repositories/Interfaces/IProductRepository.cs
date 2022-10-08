using Contracts.Common.Interfaces;
using Product.API.Entities;
using Product.API.Presistence;

namespace Product.API.Repositories.Interfaces;

public interface IProductRepository : IRepositoryBaseAsync<CatalogProduct, long, ProductContext>
{
    Task<IEnumerable<CatalogProduct>> GetProducts();
    Task<CatalogProduct> GetProductById(long Id);
    Task CreateProduct(CatalogProduct product);
    Task UpdateProduct(CatalogProduct product);
    Task DeleteProduct(long Id);
}