using System;
using Store.Domain.Exceptions;

namespace Store.Domain.Models;

public record Customer
{
    public Customer(string name)
    {
        Name = name.Trim().Length > 5 ? name.Trim() : throw new InvalidCustomerException("Name is invalid");
    }

    private Customer(Guid uuid)
    {
        Uuid = uuid != Guid.Empty ? uuid : throw new InvalidCustomerException("Uuid is empty.");
    }

    public Guid Uuid { get; set; }
    public string Name { get; set; } = string.Empty;

    public static implicit operator Customer(Guid uuid)
        => new(uuid);
}