namespace AddressBook.Application.Exceptions;

/// <summary>
/// Thrown when a requested resource does not exist; translated to HTTP 404 by the API.
/// </summary>
public class NotFoundException(string message) : Exception(message);
