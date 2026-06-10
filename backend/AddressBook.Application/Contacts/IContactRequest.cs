namespace AddressBook.Application.Contacts;

/// <summary>
/// Common shape of create/update contact requests, so validation rules are defined once.
/// </summary>
public interface IContactRequest
{
    string? FirstName { get; }

    string? LastName { get; }

    string? Address { get; }

    string? PhoneNumber { get; }
}
