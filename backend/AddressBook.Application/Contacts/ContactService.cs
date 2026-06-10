using AddressBook.Application.Common;
using AddressBook.Application.Entities;
using AddressBook.Application.Exceptions;
using AddressBook.Application.Repositories;
using FluentValidation;

namespace AddressBook.Application.Contacts;

public class ContactService(
    IContactRepository contactRepository,
    IValidator<CreateContactRequest> createValidator,
    IValidator<UpdateContactRequest> updateValidator) : IContactService
{
    public async Task<PagedResult<ContactDto>> GetContactsAsync(ContactQuery query, CancellationToken cancellationToken = default)
    {
        var page = Math.Max(query.Page, 1);
        var pageSize = Math.Clamp(query.PageSize, 1, ContactQuery.MaxPageSize);

        var filter = new ContactFilter(query.FirstName, query.LastName, query.Address, query.PhoneNumber);

        var (items, totalCount) = await contactRepository.GetPagedAsync(filter, page, pageSize, cancellationToken);

        return new PagedResult<ContactDto>(
            items.Select(c => c.ToDto()).ToList(),
            totalCount,
            page,
            pageSize);
    }

    public async Task<ContactDto> GetContactAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var contact = await GetRequiredContactAsync(id, cancellationToken);

        return contact.ToDto();
    }

    public async Task<ContactDto> CreateContactAsync(CreateContactRequest request, CancellationToken cancellationToken = default)
    {
        await createValidator.ValidateAndThrowAsync(request, cancellationToken);

        var phoneNumber = request.PhoneNumber!.Trim();
        await EnsurePhoneNumberIsAvailableAsync(phoneNumber, excludeContactId: null, cancellationToken);

        var contact = new Contact
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName!.Trim(),
            LastName = request.LastName!.Trim(),
            Address = request.Address!.Trim(),
            PhoneNumber = phoneNumber,
        };

        await contactRepository.AddAsync(contact, cancellationToken);

        return contact.ToDto();
    }

    public async Task<ContactDto> UpdateContactAsync(Guid id, UpdateContactRequest request, CancellationToken cancellationToken = default)
    {
        await updateValidator.ValidateAndThrowAsync(request, cancellationToken);

        var contact = await GetRequiredContactAsync(id, cancellationToken);

        var phoneNumber = request.PhoneNumber!.Trim();
        await EnsurePhoneNumberIsAvailableAsync(phoneNumber, excludeContactId: id, cancellationToken);

        contact.FirstName = request.FirstName!.Trim();
        contact.LastName = request.LastName!.Trim();
        contact.Address = request.Address!.Trim();
        contact.PhoneNumber = phoneNumber;

        await contactRepository.UpdateAsync(contact, cancellationToken);

        return contact.ToDto();
    }

    public async Task DeleteContactAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var contact = await GetRequiredContactAsync(id, cancellationToken);

        await contactRepository.DeleteAsync(contact, cancellationToken);
    }

    private async Task<Contact> GetRequiredContactAsync(Guid id, CancellationToken cancellationToken)
    {
        var contact = await contactRepository.GetByIdAsync(id, cancellationToken);

        return contact ?? throw new NotFoundException($"Contact with id '{id}' was not found.");
    }

    private async Task EnsurePhoneNumberIsAvailableAsync(string phoneNumber, Guid? excludeContactId, CancellationToken cancellationToken)
    {
        if (await contactRepository.PhoneNumberExistsAsync(phoneNumber, excludeContactId, cancellationToken))
        {
            throw new ConflictException($"A contact with phone number '{phoneNumber}' already exists.");
        }
    }
}
