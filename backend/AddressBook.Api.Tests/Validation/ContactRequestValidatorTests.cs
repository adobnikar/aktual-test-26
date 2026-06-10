using AddressBook.Application.Contacts;
using AddressBook.Application.Contacts.Validation;
using AddressBook.Application.Entities;
using FluentValidation.TestHelper;

namespace AddressBook.Api.Tests.Validation;

public class ContactRequestValidatorTests
{
    private readonly CreateContactRequestValidator _validator = new();

    private static CreateContactRequest ValidRequest() => new(
        FirstName: "John",
        LastName: "Smith",
        Address: "12 Baker Street, London",
        PhoneNumber: "+44 20 7946 0001");

    [Fact]
    public void Valid_request_passes()
    {
        var result = _validator.TestValidate(ValidRequest());

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Missing_first_name_fails(string? firstName)
    {
        var request = ValidRequest() with { FirstName = firstName };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.FirstName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Missing_last_name_fails(string? lastName)
    {
        var request = ValidRequest() with { LastName = lastName };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.LastName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Missing_address_fails(string? address)
    {
        var request = ValidRequest() with { Address = address };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.Address);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("not-a-phone")]
    [InlineData("++123")]
    public void Missing_or_malformed_phone_number_fails(string? phoneNumber)
    {
        var request = ValidRequest() with { PhoneNumber = phoneNumber };

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.PhoneNumber);
    }

    [Theory]
    [InlineData("+386 40 123 456")]
    [InlineData("040-123-456")]
    [InlineData("(01) 234 56 78")]
    public void Common_phone_number_formats_pass(string phoneNumber)
    {
        var request = ValidRequest() with { PhoneNumber = phoneNumber };

        var result = _validator.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(r => r.PhoneNumber);
    }

    [Fact]
    public void Too_long_fields_fail()
    {
        var request = new CreateContactRequest(
            FirstName: new string('a', ContactFieldLengths.FirstName + 1),
            LastName: new string('b', ContactFieldLengths.LastName + 1),
            Address: new string('c', ContactFieldLengths.Address + 1),
            PhoneNumber: "+" + new string('1', ContactFieldLengths.PhoneNumber));

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(r => r.FirstName);
        result.ShouldHaveValidationErrorFor(r => r.LastName);
        result.ShouldHaveValidationErrorFor(r => r.Address);
        result.ShouldHaveValidationErrorFor(r => r.PhoneNumber);
    }
}
