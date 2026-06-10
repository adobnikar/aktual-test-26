namespace AddressBook.Application.Repositories;

/// <summary>
/// Per-field search terms for querying contacts; null or empty fields are not filtered on.
/// </summary>
public record ContactFilter(
    string? FirstName = null,
    string? LastName = null,
    string? Address = null,
    string? PhoneNumber = null);
