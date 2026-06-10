using AddressBook.Application.Entities;

namespace AddressBook.Application.Repositories;

public interface IContactRepository
{
    Task<Contact?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<Contact> Items, int TotalCount)> GetPagedAsync(
        ContactFilter filter,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<bool> PhoneNumberExistsAsync(string phoneNumber, Guid? excludeContactId = null, CancellationToken cancellationToken = default);

    Task AddAsync(Contact contact, CancellationToken cancellationToken = default);

    Task UpdateAsync(Contact contact, CancellationToken cancellationToken = default);

    Task DeleteAsync(Contact contact, CancellationToken cancellationToken = default);
}
