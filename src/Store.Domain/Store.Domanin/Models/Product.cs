using Store.Domain.Exceptions;

namespace Store.Domain.Models;

public record Product
{
    public Product(Guid uuid, int quantity)
    {
        Uuid = uuid != Guid.Empty ? uuid : throw new InvalidProductException("Uuid is invalid.");
        Quantity = quantity;
    }

    public Product(string name, decimal price, int quantity)
    {
        Name = name.Trim().Length > 5 ? name.Trim() : throw new InvalidProductException("Name is invalid.");
        Price = price > 0 ? price : throw new InvalidProductException("Price is invalid.");
        Quantity = quantity >= 0 ? quantity : throw new InvalidProductException("Quantity is invalid.");
        Uuid = Guid.NewGuid();
    }

    public Guid Uuid { get; set; }

    public string Name { get; } = string.Empty!;

    public decimal Price { get; }

    public int Quantity { get; private set; }

    public void DecreaseQuantity(int quantity)
    {
        Quantity -= quantity;
    }

    public void IncreaseQuantity(int quantity)
    {
        Quantity += quantity;
    }
}