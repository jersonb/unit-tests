using System;
using Store.Domain.Exceptions;

namespace Store.Domain.Models;

public record Seller
{
    public Seller(string name)
    {
        Name = name.Trim();
    }

    private Seller(Guid uuid)
    {
        Uuid = uuid != Guid.Empty ? uuid : throw new InvalidSellerException("Uuid is empty.");
    }

    public Guid Uuid { get; set; }

    public string Name { get; set; } = string.Empty!;

    public static implicit operator Seller(Guid uuid)
        => new(uuid);
}