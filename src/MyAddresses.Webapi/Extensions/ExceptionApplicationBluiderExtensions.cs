using System;
using System.Linq;
using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyAddresses.Domain.Exceptions;

namespace MyAddresses.Webapi.Extensions
{
    public static class ExceptionApplicationBluiderExtensions
    {
        public static void UseAppExceptionHanler(this IApplicationBuilder app) =>
            app.UseExceptionHandler(handler => handler.Run(async context =>
            {
                var feature = context.Features.Get<IExceptionHandlerFeature>();
                if (feature == null) return;

                var exception = feature.Error;
                var problem = HandleProblem(exception);
                problem.Instance = context.Request.HttpContext.Request.Path;

                context.Response.StatusCode = problem.Status ?? StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/problem+json";
                await context.Response.WriteAsync(ToJson(problem));
            }));

        private static ProblemDetails HandleProblem(Exception exception) =>
            exception switch
            {
                ValidationException validationException => ValidationProblem(validationException),
                AppValidationException appValidationException => ValidationProblem(appValidationException),
                _ => ServerProblem(exception, StatusCodes.Status500InternalServerError)
            };

        private static ProblemDetails ServerProblem(Exception exception, int statusCode) =>
            new ProblemDetails
            {
                Title = exception.Message,
                Status = statusCode,
                Detail = exception.StackTrace
            };

        private static ProblemDetails ValidationProblem(AppValidationException exception) =>
            new ProblemDetails
            {
                Title = exception.Message,
                Status = exception.StatusCode,
                Detail = exception.Detail
            };

        private static ProblemDetails ValidationProblem(ValidationException exception) =>
            new ValidationProblemDetails
            {
                Title = exception.Message,
                Status = StatusCodes.Status400BadRequest,
                Errors = exception.Errors.Select(x => x.ErrorMessage)
            };

        private static string ToJson(ProblemDetails problemDetails)
        {
            if (problemDetails is ValidationProblemDetails validationProblemDetails)
                return JsonSerializer.Serialize(validationProblemDetails);
            
            return JsonSerializer.Serialize(problemDetails);
        }
    }
}