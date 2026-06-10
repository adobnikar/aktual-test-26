using AddressBook.Application.Repositories;
using AddressBook.Infrastructure.Persistence;
using AddressBook.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AddressBook.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AddressBookDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("AddressBook")));

        services.AddScoped<IContactRepository, ContactRepository>();

        return services;
    }
}
