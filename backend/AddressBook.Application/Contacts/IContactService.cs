using AddressBook.Application.Common;

namespace AddressBook.Application.Contacts;

public interface IContactService
{
    Task<PagedResult<ContactDto>> GetContactsAsync(ContactQuery query, CancellationToken cancellationToken = default);

    Task<ContactDto> GetContactAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ContactDto> CreateContactAsync(CreateContactRequest request, CancellationToken cancellationToken = default);

    Task<ContactDto> UpdateContactAsync(Guid id, UpdateContactRequest request, CancellationToken cancellationToken = default);

    Task DeleteContactAsync(Guid id, CancellationToken cancellationToken = default);
}
