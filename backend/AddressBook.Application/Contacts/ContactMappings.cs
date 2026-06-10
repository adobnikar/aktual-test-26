using AddressBook.Application.Entities;

namespace AddressBook.Application.Contacts;

public static class ContactMappings
{
    public static ContactDto ToDto(this Contact contact) => new(
        contact.Id,
        contact.FirstName,
        contact.LastName,
        contact.Address,
        contact.PhoneNumber);
}
