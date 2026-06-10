using System.Net;
using System.Net.Http.Json;
using AddressBook.Application.Common;
using AddressBook.Application.Contacts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AddressBook.Api.Tests.Integration;

public class ContactsApiTests(AddressBookApiFactory factory) : IClassFixture<AddressBookApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    private static readonly Guid SeedContactId = Guid.Parse("00000000-0000-0000-0000-000000000001");

    private static CreateContactRequest NewContact(string phoneNumber) =>
        new("Integration", "Test", "Test Street 1", phoneNumber);

    [Fact]
    public async Task List_returns_seeded_contacts_with_pagination_metadata()
    {
        var result = await _client.GetFromJsonAsync<PagedResult<ContactDto>>("/api/contacts?page=1&pageSize=10");

        Assert.NotNull(result);
        Assert.True(result.TotalCount >= 30);
        Assert.Equal(10, result.Items.Count);
        Assert.Equal(1, result.Page);
        Assert.Equal(10, result.PageSize);
    }

    [Fact]
    public async Task List_searches_each_field_separately_on_the_server()
    {
        var byFirstName = await _client.GetFromJsonAsync<PagedResult<ContactDto>>("/api/contacts?firstName=luka");
        Assert.All(byFirstName!.Items, c => Assert.Contains("luka", c.FirstName, StringComparison.OrdinalIgnoreCase));
        Assert.True(byFirstName.TotalCount >= 1);

        var byLastName = await _client.GetFromJsonAsync<PagedResult<ContactDto>>("/api/contacts?lastName=novak");
        Assert.All(byLastName!.Items, c => Assert.Contains("novak", c.LastName, StringComparison.OrdinalIgnoreCase));

        var byAddress = await _client.GetFromJsonAsync<PagedResult<ContactDto>>("/api/contacts?address=ljubljana");
        Assert.All(byAddress!.Items, c => Assert.Contains("ljubljana", c.Address, StringComparison.OrdinalIgnoreCase));
        Assert.True(byAddress.TotalCount >= 2);

        var byPhone = await _client.GetFromJsonAsync<PagedResult<ContactDto>>("/api/contacts?phoneNumber=7946");
        Assert.All(byPhone!.Items, c => Assert.Contains("7946", c.PhoneNumber));
        Assert.True(byPhone.TotalCount >= 1);
    }

    [Fact]
    public async Task List_matches_like_wildcards_literally()
    {
        var result = await _client.GetFromJsonAsync<PagedResult<ContactDto>>("/api/contacts?firstName=%25");

        Assert.Equal(0, result!.TotalCount);
    }

    [Fact]
    public async Task Get_returns_contact_for_known_id()
    {
        var contact = await _client.GetFromJsonAsync<ContactDto>($"/api/contacts/{SeedContactId}");

        Assert.NotNull(contact);
        Assert.Equal("John", contact.FirstName);
        Assert.Equal("Smith", contact.LastName);
    }

    [Fact]
    public async Task Get_returns_404_problem_for_unknown_id()
    {
        var response = await _client.GetAsync($"/api/contacts/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.Equal(StatusCodes.Status404NotFound, problem!.Status);
    }

    [Fact]
    public async Task Create_returns_201_with_location_and_contact_is_retrievable()
    {
        var response = await _client.PostAsJsonAsync("/api/contacts", NewContact("+386 31 000 001"));

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(response.Headers.Location);

        var created = await response.Content.ReadFromJsonAsync<ContactDto>();
        var fetched = await _client.GetFromJsonAsync<ContactDto>(response.Headers.Location);

        Assert.Equal(created, fetched);
    }

    [Fact]
    public async Task Create_returns_400_with_field_errors_when_fields_are_missing()
    {
        var response = await _client.PostAsJsonAsync("/api/contacts", new CreateContactRequest("", null, "x", "abc"));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        Assert.Contains("firstName", problem!.Errors.Keys);
        Assert.Contains("lastName", problem.Errors.Keys);
        Assert.Contains("phoneNumber", problem.Errors.Keys);
    }

    [Fact]
    public async Task Create_returns_409_for_duplicate_phone_number()
    {
        var first = await _client.PostAsJsonAsync("/api/contacts", NewContact("+386 31 000 002"));
        Assert.Equal(HttpStatusCode.Created, first.StatusCode);

        var duplicate = await _client.PostAsJsonAsync("/api/contacts", NewContact("+386 31 000 002"));

        Assert.Equal(HttpStatusCode.Conflict, duplicate.StatusCode);

        var problem = await duplicate.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.Equal(StatusCodes.Status409Conflict, problem!.Status);
    }

    [Fact]
    public async Task Update_changes_contact_and_returns_updated_dto()
    {
        var createResponse = await _client.PostAsJsonAsync("/api/contacts", NewContact("+386 31 000 003"));
        var created = await createResponse.Content.ReadFromJsonAsync<ContactDto>();

        var update = new UpdateContactRequest("Updated", "Person", "Updated Street 2", "+386 31 000 003");
        var response = await _client.PutAsJsonAsync($"/api/contacts/{created!.Id}", update);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var updated = await response.Content.ReadFromJsonAsync<ContactDto>();
        Assert.Equal("Updated", updated!.FirstName);

        var fetched = await _client.GetFromJsonAsync<ContactDto>($"/api/contacts/{created.Id}");
        Assert.Equal(updated, fetched);
    }

    [Fact]
    public async Task Update_returns_404_for_unknown_id()
    {
        var update = new UpdateContactRequest("Ghost", "Contact", "Nowhere 0", "+386 31 000 004");

        var response = await _client.PutAsJsonAsync($"/api/contacts/{Guid.NewGuid()}", update);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Update_returns_409_when_taking_another_contacts_phone_number()
    {
        var createResponse = await _client.PostAsJsonAsync("/api/contacts", NewContact("+386 31 000 005"));
        var created = await createResponse.Content.ReadFromJsonAsync<ContactDto>();

        // Seed contact 1 owns +44 20 7946 0001.
        var update = new UpdateContactRequest("Integration", "Test", "Test Street 1", "+44 20 7946 0001");
        var response = await _client.PutAsJsonAsync($"/api/contacts/{created!.Id}", update);

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task Delete_removes_contact_and_returns_404_afterwards()
    {
        var createResponse = await _client.PostAsJsonAsync("/api/contacts", NewContact("+386 31 000 006"));
        var created = await createResponse.Content.ReadFromJsonAsync<ContactDto>();

        var deleteResponse = await _client.DeleteAsync($"/api/contacts/{created!.Id}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var repeatedDelete = await _client.DeleteAsync($"/api/contacts/{created.Id}");
        Assert.Equal(HttpStatusCode.NotFound, repeatedDelete.StatusCode);

        var fetch = await _client.GetAsync($"/api/contacts/{created.Id}");
        Assert.Equal(HttpStatusCode.NotFound, fetch.StatusCode);
    }
}
