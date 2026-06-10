using AddressBook.Application.Entities;
using FluentValidation;

namespace AddressBook.Application.Contacts.Validation;

/// <summary>
/// Shared validation rules for create and update contact requests: all fields are required.
/// </summary>
public abstract class ContactRequestValidator<T> : AbstractValidator<T>
    where T : IContactRequest
{
    protected ContactRequestValidator()
    {
        RuleFor(r => r.FirstName)
            .NotEmpty()
            .MaximumLength(ContactFieldLengths.FirstName);

        RuleFor(r => r.LastName)
            .NotEmpty()
            .MaximumLength(ContactFieldLengths.LastName);

        RuleFor(r => r.Address)
            .NotEmpty()
            .MaximumLength(ContactFieldLengths.Address);

        RuleFor(r => r.PhoneNumber)
            .NotEmpty()
            .MaximumLength(ContactFieldLengths.PhoneNumber)
            .Matches(@"^\+?[0-9][0-9 ()\-./]*$")
            .WithMessage("'Phone Number' must be a valid phone number.");
    }
}

public class CreateContactRequestValidator : ContactRequestValidator<CreateContactRequest>;

public class UpdateContactRequestValidator : ContactRequestValidator<UpdateContactRequest>;
