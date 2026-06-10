using AddressBook.Api.Middleware;
using AddressBook.Application;
using AddressBook.Infrastructure;
using AddressBook.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

const string frontendCorsPolicy = "Frontend";

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options => options.AddPolicy(frontendCorsPolicy, policy =>
{
    var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];

    policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod();
}));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AddressBookDbContext>();
    dbContext.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseCors(frontendCorsPolicy);

app.MapControllers();

app.Run();
