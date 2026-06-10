using AddressBook.Application.Entities;
using Microsoft.EntityFrameworkCore;

namespace AddressBook.Infrastructure.Persistence;

public class AddressBookDbContext(DbContextOptions<AddressBookDbContext> options) : DbContext(options)
{
    public DbSet<Contact> Contacts => Set<Contact>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(c => c.Id);

            entity.Property(c => c.FirstName).IsRequired().HasMaxLength(ContactFieldLengths.FirstName);
            entity.Property(c => c.LastName).IsRequired().HasMaxLength(ContactFieldLengths.LastName);
            entity.Property(c => c.Address).IsRequired().HasMaxLength(ContactFieldLengths.Address);
            entity.Property(c => c.PhoneNumber).IsRequired().HasMaxLength(ContactFieldLengths.PhoneNumber);

            entity.HasIndex(c => c.PhoneNumber).IsUnique();

            entity.HasData(ContactSeed.Contacts);
        });
    }
}
