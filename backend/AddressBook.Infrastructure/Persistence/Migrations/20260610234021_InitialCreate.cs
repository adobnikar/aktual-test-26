using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AddressBook.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Contacts",
                columns: new[] { "Id", "Address", "FirstName", "LastName", "PhoneNumber" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), "12 Baker Street, London", "John", "Smith", "+44 20 7946 0001" },
                    { new Guid("00000000-0000-0000-0000-000000000002"), "45 Oxford Road, Manchester", "Emma", "Johnson", "+44 161 496 0002" },
                    { new Guid("00000000-0000-0000-0000-000000000003"), "Slovenska cesta 10, Ljubljana", "Luka", "Novak", "+386 1 234 0003" },
                    { new Guid("00000000-0000-0000-0000-000000000004"), "Celovska cesta 25, Ljubljana", "Ana", "Horvat", "+386 1 234 0004" },
                    { new Guid("00000000-0000-0000-0000-000000000005"), "Via Roma 3, Trieste", "Marco", "Rossi", "+39 040 555 0005" },
                    { new Guid("00000000-0000-0000-0000-000000000006"), "Via Garibaldi 18, Milano", "Giulia", "Bianchi", "+39 02 555 0006" },
                    { new Guid("00000000-0000-0000-0000-000000000007"), "Hauptstraße 7, Berlin", "Hans", "Müller", "+49 30 555 0007" },
                    { new Guid("00000000-0000-0000-0000-000000000008"), "Bahnhofstraße 21, München", "Anna", "Schmidt", "+49 89 555 0008" },
                    { new Guid("00000000-0000-0000-0000-000000000009"), "Rue de Rivoli 14, Paris", "Pierre", "Dubois", "+33 1 4555 0009" },
                    { new Guid("00000000-0000-0000-0000-000000000010"), "Avenue Victor Hugo 8, Lyon", "Marie", "Laurent", "+33 4 7855 0010" },
                    { new Guid("00000000-0000-0000-0000-000000000011"), "Calle Mayor 5, Madrid", "Carlos", "García", "+34 91 555 0011" },
                    { new Guid("00000000-0000-0000-0000-000000000012"), "Gran Vía 32, Barcelona", "Lucía", "Martínez", "+34 93 555 0012" },
                    { new Guid("00000000-0000-0000-0000-000000000013"), "Damstraat 9, Amsterdam", "Jan", "de Vries", "+31 20 555 0013" },
                    { new Guid("00000000-0000-0000-0000-000000000014"), "Kerkstraat 11, Utrecht", "Sanne", "Bakker", "+31 30 555 0014" },
                    { new Guid("00000000-0000-0000-0000-000000000015"), "Storgatan 2, Stockholm", "Erik", "Andersson", "+46 8 555 0015" },
                    { new Guid("00000000-0000-0000-0000-000000000016"), "Karl Johans gate 4, Oslo", "Ingrid", "Larsen", "+47 22 55 0016" },
                    { new Guid("00000000-0000-0000-0000-000000000017"), "742 Evergreen Terrace, Springfield", "James", "Brown", "+1 217 555 0017" },
                    { new Guid("00000000-0000-0000-0000-000000000018"), "350 Fifth Avenue, New York", "Olivia", "Davis", "+1 212 555 0018" },
                    { new Guid("00000000-0000-0000-0000-000000000019"), "221 Queen Street, Toronto", "Liam", "Wilson", "+1 416 555 0019" },
                    { new Guid("00000000-0000-0000-0000-000000000020"), "88 George Street, Sydney", "Sophia", "Taylor", "+61 2 5550 0020" },
                    { new Guid("00000000-0000-0000-0000-000000000021"), "Trubarjeva ulica 33, Maribor", "Peter", "Kovač", "+386 2 234 0021" },
                    { new Guid("00000000-0000-0000-0000-000000000022"), "Prešernova ulica 1, Kranj", "Maja", "Zupančič", "+386 4 234 0022" },
                    { new Guid("00000000-0000-0000-0000-000000000023"), "Václavské náměstí 12, Praha", "Tomáš", "Novák", "+420 2 5550 0023" },
                    { new Guid("00000000-0000-0000-0000-000000000024"), "ul. Marszałkowska 6, Warszawa", "Katarzyna", "Kowalska", "+48 22 555 0024" },
                    { new Guid("00000000-0000-0000-0000-000000000025"), "Váci utca 19, Budapest", "András", "Nagy", "+36 1 555 0025" },
                    { new Guid("00000000-0000-0000-0000-000000000026"), "Strada Lipscani 27, București", "Elena", "Popescu", "+40 21 555 0026" },
                    { new Guid("00000000-0000-0000-0000-000000000027"), "Ermou 15, Athens", "Dimitris", "Papadopoulos", "+30 21 0555 0027" },
                    { new Guid("00000000-0000-0000-0000-000000000028"), "1-2-3 Shibuya, Tokyo", "Yuki", "Tanaka", "+81 3 5550 0028" },
                    { new Guid("00000000-0000-0000-0000-000000000029"), "Nanjing Road 100, Shanghai", "Wei", "Chen", "+86 21 5550 0029" },
                    { new Guid("00000000-0000-0000-0000-000000000030"), "Linking Road 55, Mumbai", "Aisha", "Khan", "+91 22 5550 0030" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_PhoneNumber",
                table: "Contacts",
                column: "PhoneNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contacts");
        }
    }
}
