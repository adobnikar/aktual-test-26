using System.Text.Json;
using AddressBook.Application.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace AddressBook.Api.Middleware;

/// <summary>
/// Translates exceptions into RFC 7807 problem-details responses with standard HTTP status codes.
/// </summary>
public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var problemDetails = exception switch
        {
            ValidationException validationException => CreateValidationProblem(validationException),
            NotFoundException notFoundException => CreateProblem(StatusCodes.Status404NotFound, "Not Found", notFoundException.Message),
            ConflictException conflictException => CreateProblem(StatusCodes.Status409Conflict, "Conflict", conflictException.Message),
            DbUpdateException { InnerException: PostgresException { SqlState: PostgresErrorCodes.UniqueViolation } } =>
                CreateProblem(StatusCodes.Status409Conflict, "Conflict", "A contact with the same phone number already exists."),
            _ => CreateProblem(StatusCodes.Status500InternalServerError, "Internal Server Error", "An unexpected error occurred."),
        };

        if (problemDetails.Status == StatusCodes.Status500InternalServerError)
        {
            logger.LogError(exception, "Unhandled exception while processing {Method} {Path}", context.Request.Method, context.Request.Path);
        }

        context.Response.StatusCode = problemDetails.Status!.Value;
        context.Response.ContentType = "application/problem+json";

        await context.Response.WriteAsJsonAsync(problemDetails, problemDetails.GetType());
    }

    private static ProblemDetails CreateProblem(int status, string title, string detail) => new()
    {
        Status = status,
        Title = title,
        Detail = detail,
    };

    private static ValidationProblemDetails CreateValidationProblem(ValidationException exception)
    {
        var errors = exception.Errors
            .GroupBy(e => JsonNamingPolicy.CamelCase.ConvertName(e.PropertyName))
            .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

        return new ValidationProblemDetails(errors)
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Validation Failed",
            Detail = "One or more validation errors occurred.",
        };
    }
}
