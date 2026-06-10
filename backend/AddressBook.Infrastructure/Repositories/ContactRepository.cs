using AddressBook.Application.Entities;
using AddressBook.Application.Repositories;
using AddressBook.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AddressBook.Infrastructure.Repositories;

public class ContactRepository(AddressBookDbContext dbContext) : IContactRepository
{
    public Task<Contact?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return dbContext.Contacts.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<(IReadOnlyList<Contact> Items, int TotalCount)> GetPagedAsync(
        ContactFilter filter,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Contact> query = dbContext.Contacts.AsNoTracking();

        query = ApplyFilter(query, filter);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(c => c.LastName)
            .ThenBy(c => c.FirstName)
            .ThenBy(c => c.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public Task<bool> PhoneNumberExistsAsync(string phoneNumber, Guid? excludeContactId = null, CancellationToken cancellationToken = default)
    {
        return dbContext.Contacts.AnyAsync(
            c => c.PhoneNumber == phoneNumber && (excludeContactId == null || c.Id != excludeContactId),
            cancellationToken);
    }

    public async Task AddAsync(Contact contact, CancellationToken cancellationToken = default)
    {
        dbContext.Contacts.Add(contact);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task UpdateAsync(Contact contact, CancellationToken cancellationToken = default)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Contact contact, CancellationToken cancellationToken = default)
    {
        dbContext.Contacts.Remove(contact);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static IQueryable<Contact> ApplyFilter(IQueryable<Contact> query, ContactFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.FirstName))
        {
            query = query.Where(c => EF.Functions.ILike(c.FirstName, ContainsPattern(filter.FirstName)));
        }

        if (!string.IsNullOrWhiteSpace(filter.LastName))
        {
            query = query.Where(c => EF.Functions.ILike(c.LastName, ContainsPattern(filter.LastName)));
        }

        if (!string.IsNullOrWhiteSpace(filter.Address))
        {
            query = query.Where(c => EF.Functions.ILike(c.Address, ContainsPattern(filter.Address)));
        }

        if (!string.IsNullOrWhiteSpace(filter.PhoneNumber))
        {
            query = query.Where(c => EF.Functions.ILike(c.PhoneNumber, ContainsPattern(filter.PhoneNumber)));
        }

        return query;
    }

    private static string ContainsPattern(string term)
    {
        // Escape LIKE wildcards so search terms are matched literally.
        var escaped = term.Trim()
            .Replace(@"\", @"\\")
            .Replace("%", @"\%")
            .Replace("_", @"\_");

        return $"%{escaped}%";
    }
}
