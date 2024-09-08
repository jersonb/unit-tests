using Store.Domain.Models;

namespace Store.Domain.Contracts;

public interface IProductRepository
{
    bool CheckStock(Product product);
    bool DownStock(List<Product> saleProducts);
}