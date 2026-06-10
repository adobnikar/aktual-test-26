using AddressBook.Application.Entities;

namespace AddressBook.Infrastructure.Persistence;

/// <summary>
/// Static seed data; IDs and values must stay deterministic so EF migrations remain stable.
/// </summary>
public static class ContactSeed
{
    public static IReadOnlyList<Contact> Contacts { get; } =
    [
        New("00000000-0000-0000-0000-000000000001", "John", "Smith", "12 Baker Street, London", "+44 20 7946 0001"),
        New("00000000-0000-0000-0000-000000000002", "Emma", "Johnson", "45 Oxford Road, Manchester", "+44 161 496 0002"),
        New("00000000-0000-0000-0000-000000000003", "Luka", "Novak", "Slovenska cesta 10, Ljubljana", "+386 1 234 0003"),
        New("00000000-0000-0000-0000-000000000004", "Ana", "Horvat", "Celovska cesta 25, Ljubljana", "+386 1 234 0004"),
        New("00000000-0000-0000-0000-000000000005", "Marco", "Rossi", "Via Roma 3, Trieste", "+39 040 555 0005"),
        New("00000000-0000-0000-0000-000000000006", "Giulia", "Bianchi", "Via Garibaldi 18, Milano", "+39 02 555 0006"),
        New("00000000-0000-0000-0000-000000000007", "Hans", "Müller", "Hauptstraße 7, Berlin", "+49 30 555 0007"),
        New("00000000-0000-0000-0000-000000000008", "Anna", "Schmidt", "Bahnhofstraße 21, München", "+49 89 555 0008"),
        New("00000000-0000-0000-0000-000000000009", "Pierre", "Dubois", "Rue de Rivoli 14, Paris", "+33 1 4555 0009"),
        New("00000000-0000-0000-0000-000000000010", "Marie", "Laurent", "Avenue Victor Hugo 8, Lyon", "+33 4 7855 0010"),
        New("00000000-0000-0000-0000-000000000011", "Carlos", "García", "Calle Mayor 5, Madrid", "+34 91 555 0011"),
        New("00000000-0000-0000-0000-000000000012", "Lucía", "Martínez", "Gran Vía 32, Barcelona", "+34 93 555 0012"),
        New("00000000-0000-0000-0000-000000000013", "Jan", "de Vries", "Damstraat 9, Amsterdam", "+31 20 555 0013"),
        New("00000000-0000-0000-0000-000000000014", "Sanne", "Bakker", "Kerkstraat 11, Utrecht", "+31 30 555 0014"),
        New("00000000-0000-0000-0000-000000000015", "Erik", "Andersson", "Storgatan 2, Stockholm", "+46 8 555 0015"),
        New("00000000-0000-0000-0000-000000000016", "Ingrid", "Larsen", "Karl Johans gate 4, Oslo", "+47 22 55 0016"),
        New("00000000-0000-0000-0000-000000000017", "James", "Brown", "742 Evergreen Terrace, Springfield", "+1 217 555 0017"),
        New("00000000-0000-0000-0000-000000000018", "Olivia", "Davis", "350 Fifth Avenue, New York", "+1 212 555 0018"),
        New("00000000-0000-0000-0000-000000000019", "Liam", "Wilson", "221 Queen Street, Toronto", "+1 416 555 0019"),
        New("00000000-0000-0000-0000-000000000020", "Sophia", "Taylor", "88 George Street, Sydney", "+61 2 5550 0020"),
        New("00000000-0000-0000-0000-000000000021", "Peter", "Kovač", "Trubarjeva ulica 33, Maribor", "+386 2 234 0021"),
        New("00000000-0000-0000-0000-000000000022", "Maja", "Zupančič", "Prešernova ulica 1, Kranj", "+386 4 234 0022"),
        New("00000000-0000-0000-0000-000000000023", "Tomáš", "Novák", "Václavské náměstí 12, Praha", "+420 2 5550 0023"),
        New("00000000-0000-0000-0000-000000000024", "Katarzyna", "Kowalska", "ul. Marszałkowska 6, Warszawa", "+48 22 555 0024"),
        New("00000000-0000-0000-0000-000000000025", "András", "Nagy", "Váci utca 19, Budapest", "+36 1 555 0025"),
        New("00000000-0000-0000-0000-000000000026", "Elena", "Popescu", "Strada Lipscani 27, București", "+40 21 555 0026"),
        New("00000000-0000-0000-0000-000000000027", "Dimitris", "Papadopoulos", "Ermou 15, Athens", "+30 21 0555 0027"),
        New("00000000-0000-0000-0000-000000000028", "Yuki", "Tanaka", "1-2-3 Shibuya, Tokyo", "+81 3 5550 0028"),
        New("00000000-0000-0000-0000-000000000029", "Wei", "Chen", "Nanjing Road 100, Shanghai", "+86 21 5550 0029"),
        New("00000000-0000-0000-0000-000000000030", "Aisha", "Khan", "Linking Road 55, Mumbai", "+91 22 5550 0030"),
    ];

    private static Contact New(string id, string firstName, string lastName, string address, string phoneNumber) => new()
    {
        Id = Guid.Parse(id),
        FirstName = firstName,
        LastName = lastName,
        Address = address,
        PhoneNumber = phoneNumber,
    };
}
