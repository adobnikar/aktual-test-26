using AddressBook.Application.Contacts;
using AddressBook.Application.Contacts.Validation;
using AddressBook.Application.Entities;
using AddressBook.Application.Exceptions;
using AddressBook.Application.Repositories;
using FluentValidation;
using NSubstitute;

namespace AddressBook.Api.Tests.Contacts;

public class ContactServiceTests
{
    private readonly IContactRepository _repository = Substitute.For<IContactRepository>();
    private readonly ContactService _service;

    public ContactServiceTests()
    {
        _service = new ContactService(_repository, new CreateContactRequestValidator(), new UpdateContactRequestValidator());
    }

    private static Contact ExistingContact() => new()
    {
        Id = Guid.NewGuid(),
        FirstName = "John",
        LastName = "Smith",
        Address = "12 Baker Street, London",
        PhoneNumber = "+44 20 7946 0001",
    };

    [Fact]
    public async Task GetContact_throws_not_found_for_unknown_id()
    {
        _repository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((Contact?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _service.GetContactAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task CreateContact_throws_validation_exception_for_missing_fields()
    {
        var request = new CreateContactRequest(null, "", "  ", null);

        await Assert.ThrowsAsync<ValidationException>(() => _service.CreateContactAsync(request));

        await _repository.DidNotReceive().AddAsync(Arg.Any<Contact>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateContact_throws_conflict_for_duplicate_phone_number()
    {
        _repository.PhoneNumberExistsAsync("+44 20 7946 0001", null, Arg.Any<CancellationToken>()).Returns(true);

        var request = new CreateContactRequest("Jane", "Doe", "Elm Street 3", "+44 20 7946 0001");

        await Assert.ThrowsAsync<ConflictException>(() => _service.CreateContactAsync(request));

        await _repository.DidNotReceive().AddAsync(Arg.Any<Contact>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateContact_trims_fields_and_persists_contact()
    {
        var request = new CreateContactRequest("  Jane ", " Doe ", " Elm Street 3 ", " +44 20 7946 0002 ");

        var dto = await _service.CreateContactAsync(request);

        Assert.Equal("Jane", dto.FirstName);
        Assert.Equal("Doe", dto.LastName);
        Assert.Equal("Elm Street 3", dto.Address);
        Assert.Equal("+44 20 7946 0002", dto.PhoneNumber);
        Assert.NotEqual(Guid.Empty, dto.Id);

        await _repository.Received(1).AddAsync(
            Arg.Is<Contact>(c => c.Id == dto.Id && c.PhoneNumber == "+44 20 7946 0002"),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateContact_throws_not_found_for_unknown_id()
    {
        _repository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((Contact?)null);

        var request = new UpdateContactRequest("Jane", "Doe", "Elm Street 3", "+44 20 7946 0002");

        await Assert.ThrowsAsync<NotFoundException>(() => _service.UpdateContactAsync(Guid.NewGuid(), request));
    }

    [Fact]
    public async Task UpdateContact_throws_conflict_when_phone_number_belongs_to_another_contact()
    {
        var contact = ExistingContact();
        _repository.GetByIdAsync(contact.Id, Arg.Any<CancellationToken>()).Returns(contact);
        _repository.PhoneNumberExistsAsync("+44 20 7946 0009", contact.Id, Arg.Any<CancellationToken>()).Returns(true);

        var request = new UpdateContactRequest("John", "Smith", "12 Baker Street, London", "+44 20 7946 0009");

        await Assert.ThrowsAsync<ConflictException>(() => _service.UpdateContactAsync(contact.Id, request));

        await _repository.DidNotReceive().UpdateAsync(Arg.Any<Contact>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateContact_applies_changes_and_persists()
    {
        var contact = ExistingContact();
        _repository.GetByIdAsync(contact.Id, Arg.Any<CancellationToken>()).Returns(contact);

        var request = new UpdateContactRequest("Johnny", "Smithers", "New Street 9", contact.PhoneNumber);

        var dto = await _service.UpdateContactAsync(contact.Id, request);

        Assert.Equal("Johnny", dto.FirstName);
        Assert.Equal("Smithers", dto.LastName);
        Assert.Equal("New Street 9", dto.Address);

        await _repository.Received(1).UpdateAsync(contact, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DeleteContact_throws_not_found_for_unknown_id()
    {
        _repository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((Contact?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _service.DeleteContactAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task DeleteContact_removes_existing_contact()
    {
        var contact = ExistingContact();
        _repository.GetByIdAsync(contact.Id, Arg.Any<CancellationToken>()).Returns(contact);

        await _service.DeleteContactAsync(contact.Id);

        await _repository.Received(1).DeleteAsync(contact, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetContacts_clamps_page_and_page_size()
    {
        _repository.GetPagedAsync(Arg.Any<ContactFilter>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(((IReadOnlyList<Contact>)[], 0));

        var query = new ContactQuery { Page = -5, PageSize = 5000 };

        var result = await _service.GetContactsAsync(query);

        Assert.Equal(1, result.Page);
        Assert.Equal(ContactQuery.MaxPageSize, result.PageSize);

        await _repository.Received(1).GetPagedAsync(Arg.Any<ContactFilter>(), 1, ContactQuery.MaxPageSize, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetContacts_passes_search_terms_to_repository()
    {
        _repository.GetPagedAsync(Arg.Any<ContactFilter>(), Arg.Any<int>(), Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(((IReadOnlyList<Contact>)[ExistingContact()], 1));

        var query = new ContactQuery { FirstName = "jo", LastName = "sm", Address = "baker", PhoneNumber = "7946" };

        var result = await _service.GetContactsAsync(query);

        Assert.Equal(1, result.TotalCount);
        Assert.Single(result.Items);

        await _repository.Received(1).GetPagedAsync(
            new ContactFilter("jo", "sm", "baker", "7946"),
            1,
            10,
            Arg.Any<CancellationToken>());
    }
}
