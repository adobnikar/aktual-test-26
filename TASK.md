# Fullstack Developer Test Assignment

## Things that will be tested

- Project scaffolding and organization
- Naming conventions
- Best practices (Dependency Injection, DRY, Design Patterns etc.)
- Use of external libraries

## Assignment

This assignment requires the realization of the web application **"Address book"**. The application must be able to **view, search, create, update and delete** contacts.

The contact information must include:

- First name
- Last name
- Address
- Telephone number

It should be possible to search each data separately (this should be done on the backend).

Validation must check if all data is entered, and that phone number does not already exist in database.

The application should be composed of two parts: **backend** and **frontend**.

**All code should be written in English and stored on GitHub.**

## Backend

### Elements

- Database access layer
- Data validation
- REST API (using standard HTTP status codes)

### Required technologies/tools

- .NET / C#
- MS SQL Server or PostgreSQL
- .NET Core
- EntityFramework or NHibernate (ORM)

## Frontend

### Requirements

- List view with (serverside) search and pagination
- View of a single contact with all contact's information
- Create / Update with validation (all fields required)
- Deleting – user must confirm delete (modal)
- Serverside errors should be displayed to user (modal)

### Required technologies/tools

- Angular
- Bootstrap
