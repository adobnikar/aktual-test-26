using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.PostgreSql;

namespace AddressBook.Api.Tests.Integration;

/// <summary>
/// Boots the API against a throwaway PostgreSQL container; migrations and seed run on startup.
/// </summary>
public class AddressBookApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _database = new PostgreSqlBuilder()
        .WithImage("postgres:16-alpine")
        .Build();

    public Task InitializeAsync() => _database.StartAsync();

    Task IAsyncLifetime.DisposeAsync() => _database.DisposeAsync().AsTask();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("ConnectionStrings:AddressBook", _database.GetConnectionString());
        builder.UseEnvironment("Production");
    }
}
