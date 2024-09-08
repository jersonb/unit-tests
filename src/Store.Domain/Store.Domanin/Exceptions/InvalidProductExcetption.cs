using System;

namespace Store.Domain.Exceptions;

public class InvalidProductException(string message) : Exception(message);