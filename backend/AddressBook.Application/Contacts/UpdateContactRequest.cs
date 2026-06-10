namespace AddressBook.Application.Contacts;

public record UpdateContactRequest(
    string? FirstName,
    string? LastName,
    string? Address,
    string? PhoneNumber);
