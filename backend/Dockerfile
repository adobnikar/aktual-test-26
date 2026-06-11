FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Restore first so layer caching kicks in when only source files change.
COPY AddressBook.sln .
COPY AddressBook.Api/AddressBook.Api.csproj AddressBook.Api/
COPY AddressBook.Application/AddressBook.Application.csproj AddressBook.Application/
COPY AddressBook.Infrastructure/AddressBook.Infrastructure.csproj AddressBook.Infrastructure/
RUN dotnet restore AddressBook.Api/AddressBook.Api.csproj

COPY . .
RUN dotnet publish AddressBook.Api/AddressBook.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "AddressBook.Api.dll"]
