namespace AddressBook.Application.Contacts;

public record ContactDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Address,
    string PhoneNumber);
