namespace Store.Domain.Exceptions;

public class InvalidSaleException(string message) : Exception(message);