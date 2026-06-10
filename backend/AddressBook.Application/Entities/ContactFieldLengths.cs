namespace AddressBook.Application.Entities;

/// <summary>
/// Maximum field lengths, shared between the database schema and request validation.
/// </summary>
public static class ContactFieldLengths
{
    public const int FirstName = 100;
    public const int LastName = 100;
    public const int Address = 250;
    public const int PhoneNumber = 30;
}
