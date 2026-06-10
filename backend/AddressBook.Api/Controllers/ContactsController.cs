using AddressBook.Application.Common;
using AddressBook.Application.Contacts;
using Microsoft.AspNetCore.Mvc;

namespace AddressBook.Api.Controllers;

[ApiController]
[Route("api/contacts")]
[Produces("application/json")]
public class ContactsController(IContactService contactService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<PagedResult<ContactDto>> GetContacts([FromQuery] ContactQuery query, CancellationToken cancellationToken)
    {
        return contactService.GetContactsAsync(query, cancellationToken);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<ContactDto> GetContact(Guid id, CancellationToken cancellationToken)
    {
        return contactService.GetContactAsync(id, cancellationToken);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ContactDto>> CreateContact(CreateContactRequest request, CancellationToken cancellationToken)
    {
        var contact = await contactService.CreateContactAsync(request, cancellationToken);

        return CreatedAtAction(nameof(GetContact), new { id = contact.Id }, contact);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public Task<ContactDto> UpdateContact(Guid id, UpdateContactRequest request, CancellationToken cancellationToken)
    {
        return contactService.UpdateContactAsync(id, request, cancellationToken);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteContact(Guid id, CancellationToken cancellationToken)
    {
        await contactService.DeleteContactAsync(id, cancellationToken);

        return NoContent();
    }
}
