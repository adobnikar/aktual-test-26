namespace AddressBook.Application.Contacts;

/// <summary>
/// Search and pagination parameters for listing contacts.
/// </summary>
public record ContactQuery
{
    public const int MaxPageSize = 100;

    public string? FirstName { get; init; }

    public string? LastName { get; init; }

    public string? Address { get; init; }

    public string? PhoneNumber { get; init; }

    public int Page { get; init; } = 1;

    public int PageSize { get; init; } = 10;
}
