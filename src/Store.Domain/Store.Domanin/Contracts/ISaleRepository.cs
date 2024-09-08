using Store.Domain.Models;

namespace Store.Domain.Contracts;

public interface ISaleRepository
{
    Guid Add(Sale sale);
}