using Store.Domain.Contracts;
using Store.Domain.Models;
using Store.Domain.Services;

namespace Store.Domain.Tests;

public class SaleServiceFakesTests
{
    [Fact]
    public void Should_create_sale()
    {
        var saleRepositoryFake = new SaleRepositoryFake();
        var service = new SaleService(saleRepositoryFake, new ProductRepositoryFake());
        var sellerUuid = Guid.NewGuid();
        var customerUuid = Guid.NewGuid();
        var sale = new Sale(sellerUuid, customerUuid);
        service.Create(sale);

        Assert.True(saleRepositoryFake.SalesFake.Count == 1);
    }

    [Fact]
    public void Should_add_error_in_sale_when_product_out_of_stock()
    {
        var saleRepositoryFake = new SaleRepositoryFake();
        var productRepositoryFake = new ProductRepositoryFake();
        var service = new SaleService(saleRepositoryFake, productRepositoryFake);

        Sale sale = Guid.NewGuid();
        var product = ProductRepositoryFake.ProductQuantity0;
        service.AddProduct(sale, product);

        Assert.Empty(sale.Products);
        Assert.Contains(ErrorDomain.ProductOutOfStock(product), sale.Errors);
    }

    [Fact]
    public void Should_add_product_in_sale()
    {
        var saleRepositoryFake = new SaleRepositoryFake();
        var productRepositoryFake = new ProductRepositoryFake();

        var service = new SaleService(saleRepositoryFake, productRepositoryFake);
        Sale sale = Guid.NewGuid();
        service.AddProduct(sale, ProductRepositoryFake.ProductQuantity1);

        Assert.Empty(sale.Errors);
        Assert.Single(sale.Products);
    }

    [Fact]
    public void Should_add_many_products_in_sale()
    {
        var saleRepositoryFake = new SaleRepositoryFake();
        var productRepositoryFake = new ProductRepositoryFake();
        var service = new SaleService(saleRepositoryFake, productRepositoryFake);

        Sale sale = Guid.NewGuid();

        var product1 = ProductRepositoryFake.ProductQuantity1;
        service.AddProduct(sale, product1);

        var product0 = ProductRepositoryFake.ProductQuantity0;
        service.AddProduct(sale, product0);

        var product2 = ProductRepositoryFake.ProductQuantity2;
        service.AddProduct(sale, product2);

        Assert.Contains(ErrorDomain.ProductOutOfStock(product0), sale.Errors);
        Assert.True(sale.Products.Count == 2);
    }

    [Fact]
    public void Should_decrease_product_on_finished_sale()
    {
        var saleRepositoryFake = new SaleRepositoryFake();
        Sale sale = Guid.NewGuid();
        var productRepositoryFake = new ProductRepositoryFake();
        var service = new SaleService(saleRepositoryFake, productRepositoryFake);
        var product = ProductRepositoryFake.ProductQuantity1;
        service.AddProduct(sale, product);

        service.Finish(sale);

        Assert.True(product.Quantity == 0);
    }

    [Fact]
    public void Should_not_decrease_product_when_sale_invalid()
    {
        var saleRepositoryFake = new SaleRepositoryFake();
        Sale sale = Guid.NewGuid();
        var productRepositoryFake = new ProductRepositoryFake();
        var service = new SaleService(saleRepositoryFake, productRepositoryFake);
        var product = ProductRepositoryFake.ProductQuantity0;
        service.AddProduct(sale, product);

        service.Finish(sale);

        Assert.True(product.Quantity == 0);
    }

    private class SaleRepositoryFake : ISaleRepository
    {
        public List<Sale> SalesFake { get; set; } = [];

        public Guid Add(Sale sale)
        {
            SalesFake.Add(sale);
            return sale.Uuid;
        }
    }

    private class ProductRepositoryFake : IProductRepository
    {
        public static Product ProductQuantity0 = new(Guid.NewGuid().ToString(), 1.0m, 0);
        public static Product ProductQuantity1 = new(Guid.NewGuid().ToString(), 1.0m, 1);
        public static Product ProductQuantity2 = new(Guid.NewGuid().ToString(), 1.0m, 2);
        private readonly List<Product> products;

        public ProductRepositoryFake()
        {
            ProductQuantity0.Uuid = Guid.NewGuid();
            ProductQuantity1.Uuid = Guid.NewGuid();
            ProductQuantity2.Uuid = Guid.NewGuid();
            products = [ProductQuantity0, ProductQuantity1, ProductQuantity2];
        }

        public bool CheckStock(Product product)
        {
            return products.Exists(p => p.Uuid == product.Uuid && p.Quantity >= product.Quantity && p.Quantity > 0);
        }

        public bool DownStock(List<Product> saleProducts)
        {
            var result = false;
            saleProducts.ForEach(sp =>
            {
                var product = products.Single(pr => pr.Uuid == sp.Uuid);
                product.DecreaseQuantity(sp.Quantity);
                result = true;
            });
            return result;
        }
    }
}