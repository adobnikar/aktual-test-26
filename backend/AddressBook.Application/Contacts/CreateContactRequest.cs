namespace AddressBook.Application.Contacts;

public record CreateContactRequest(
    string? FirstName,
    string? LastName,
    string? Address,
    string? PhoneNumber) : IContactRequest;
