using Store.Domain.Contracts;
using Store.Domain.Models;

namespace Store.Domain.Services;

public class SaleService(ISaleRepository saleRepository, IProductRepository productRepository)
{
    public Sale Create(Sale sale)
    {
        return saleRepository.Add(sale);
    }

    public Sale AddProduct(Sale sale, Product product)
    {
        if (productRepository.CheckStock(product))
        {
            sale.Products.Add(product);
        }
        else
        {
            sale.Errors.Add(ErrorDomain.ProductOutOfStock(product));
        }

        return sale;
    }

    public bool Finish(Sale sale)
    {
        if (!SaleIsValid(sale))
        {
            return false;
        }

        var downStock = productRepository.DownStock(sale.Products);
        var register = saleRepository.Add(sale);
        return downStock && register != Guid.Empty;
    }

    private static bool SaleIsValid(Sale sale)
    {
        var totalGreaterThanZero = sale.Total > 0;
        var hasProducts = sale.Products.Count > 0;
        return totalGreaterThanZero && hasProducts;
    }
}