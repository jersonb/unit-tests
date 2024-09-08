namespace Store.Domain.Exceptions;

public class InvalidCustomerException(string message) : Exception(message);