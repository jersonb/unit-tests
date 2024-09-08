using Store.Domain.Exceptions;

namespace Store.Domain.Models;

public class Sale
{
    public Sale(Seller seller, Customer customer)
    {
        Seller = seller ?? throw new InvalidSaleException("Seller is default.");
        Customer = customer ?? throw new InvalidSaleException("Customer is default.");
        Uuid = Guid.NewGuid();
    }

    private Sale(Guid uuid)
    {
        Uuid = uuid != Guid.Empty ? uuid : throw new InvalidSaleException("Uuid is empty.");
    }

    public Guid Uuid { get; set; }

    public Seller Seller { get; } = default!;

    public Customer Customer { get; } = default!;

    public List<Product> Products { get; set; } = [];

    public List<ErrorDomain> Errors { get; set; } = [];

    public decimal Total => Products.Sum(p => p.Price);

    public static implicit operator Sale(Guid uuid)
        => new(uuid);
}