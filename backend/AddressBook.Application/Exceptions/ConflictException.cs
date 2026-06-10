namespace AddressBook.Application.Exceptions;

/// <summary>
/// Thrown when a request conflicts with existing data; translated to HTTP 409 by the API.
/// </summary>
public class ConflictException(string message) : Exception(message);
