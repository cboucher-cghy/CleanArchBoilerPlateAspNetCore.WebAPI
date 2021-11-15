using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using CleanArchBoilerPlateAspNetCore.Core.Application.Exceptions;
using System;
using System.Net;
using System.Threading.Tasks;

namespace CleanArchBoilerPlateAspNetCore.WebAPI.Middlewares
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger<GlobalErrorHandlingMiddleware> _logger;

        public GlobalErrorHandlingMiddleware(RequestDelegate next, ILogger<GlobalErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (NotFoundException ex)
            {
                await SendErrorDetails(context, ex, HttpStatusCode.BadRequest);
            }
            catch (DuplicateValueException ex)
            {
                await SendErrorDetails(context, ex, HttpStatusCode.Conflict);
            }
            catch (InvalidValueException ex)
            {
                await SendErrorDetails(context, ex, HttpStatusCode.BadRequest);
            }
            catch (NotUpdatableException ex)
            {
                await SendErrorDetails(context, ex, HttpStatusCode.BadRequest);
            }
            catch (NotDeletableException ex)
            {
                await SendErrorDetails(context, ex, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                await SendErrorDetails(context, ex, HttpStatusCode.InternalServerError);
            }
        }

        private async Task SendErrorDetails(HttpContext context, Exception exception, HttpStatusCode httpStatusCode)
        {
            _logger.LogError(exception, "An exception as occurred.");
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)httpStatusCode;

            var errorResponse = new
            {
                message = exception.Message,
                statusCode = response.StatusCode
            };
            await response.WriteAsJsonAsync(errorResponse);
        }
    }


    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class GlobalErrorHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalErrorHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalErrorHandlingMiddleware>();
        }
    }
}