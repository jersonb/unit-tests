namespace Store.Domain.Models;

public record ErrorDomain
{
    private ErrorDomain(int code, string description, object? data = null)
    {
        Code = code;
        Description = description;
        Data = data;
    }

    public int Code { get; }
    public string Description { get; }

    public object? Data { get; }

    public static ErrorDomain ProductOutOfStock(object data = null!)
        => new(1, "Product out of stock.", data);
}