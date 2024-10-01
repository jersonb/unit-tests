using Moq;
using Store.Domain.Contracts;
using Store.Domain.Models;
using Store.Domain.Services;

namespace Store.Domain.Tests;

public class SaleServiceMocksTests
{
    [Fact]
    public void Should_create_sale()
    {
        var saleRepository = new Mock<ISaleRepository>();
        saleRepository.Setup(s => s.Add(It.IsAny<Sale>()))
            .Returns(Guid.NewGuid());
        var service = new SaleService(saleRepository.Object, Mock.Of<IProductRepository>());
        var sellerUuid = Guid.NewGuid();
        var customerUuid = Guid.NewGuid();
        var sale = new Sale(sellerUuid, customerUuid);
        service.Create(sale);
        saleRepository.Verify(s => s.Add(It.Is<Sale>(s => s == sale)));
    }

    [Fact]
    public void Should_add_error_in_sale_when_product_out_of_stock()
    {
        var productRepository = new Mock<IProductRepository>();
        productRepository.Setup(r => r.CheckStock(It.IsAny<Product>()))
            .Returns(false);

        var service = new SaleService(Mock.Of<ISaleRepository>(), productRepository.Object);
        Sale sale = Guid.NewGuid();
        var result = service.AddProduct(sale, It.IsAny<Product>());
        Assert.Empty(sale.Products);
        Assert.Contains("Product out of stock.", result.Errors[0].Description);
    }

    [Fact]
    public void Should_add_product_in_sale()
    {
        var productRepository = new Mock<IProductRepository>();
        productRepository.Setup(r => r.CheckStock(It.IsAny<Product>()))
            .Returns(true);

        var service = new SaleService(Mock.Of<ISaleRepository>(), productRepository.Object);
        Sale sale = Guid.NewGuid();
        var result = service.AddProduct(sale, It.IsAny<Product>());

        Assert.Empty(sale.Errors);
        Assert.Single(result.Products);
    }

    [Fact]
    public void Should_add_many_products_in_sale()
    {
        var productRepository = new Mock<IProductRepository>();
        productRepository.SetupSequence(p => p.CheckStock(It.IsAny<Product>()))
            .Returns(true)
            .Returns(false)
            .Returns(true);
        var service = new SaleService(Mock.Of<ISaleRepository>(), productRepository.Object);

        Sale sale = Guid.NewGuid();

        var products = new List<Product>
        {
            new(Guid.NewGuid().ToString(), 1.0m, 1),
            new(Guid.NewGuid().ToString(), 1.0m, 0),
            new(Guid.NewGuid().ToString(), 1.0m, 2),
        };

        foreach (var product in products)
        {
            service.AddProduct(sale, product);
        }

        Assert.Contains("Product out of stock.", sale.Errors[0].Description);
        Assert.True(sale.Products.Count == 2);
    }

    [Fact]
    public void Should_decrease_product_on_finished_sale()
    {
        var saleRepository = new Mock<ISaleRepository>();
        saleRepository.Setup(r => r.Add(It.IsAny<Sale>()))
            .Returns(Guid.NewGuid());

        var productRepository = new Mock<IProductRepository>();
        productRepository.Setup(r => r.DownStock(It.IsAny<List<Product>>()))
            .Returns(true);

        var service = new SaleService(saleRepository.Object, productRepository.Object);

        Sale sale = Guid.NewGuid();
        sale.Products.Add(new Product(Guid.NewGuid().ToString(), 1, 1));
        var result = service.Finish(sale);

        Assert.True(result);
    }

    [Fact]
    public void Should_not_decrease_product_when_sale_invalid()
    {
        var saleRepository = new Mock<ISaleRepository>();
        saleRepository.Setup(r => r.Add(It.IsAny<Sale>()))
            .Returns(Guid.NewGuid());

        var productRepository = new Mock<IProductRepository>();
        productRepository.Setup(r => r.DownStock(It.IsAny<List<Product>>()))
            .Returns(true);

        var service = new SaleService(saleRepository.Object, productRepository.Object);

        Sale sale = Guid.NewGuid();
        var result = service.Finish(sale);

        Assert.False(result);
    }
}