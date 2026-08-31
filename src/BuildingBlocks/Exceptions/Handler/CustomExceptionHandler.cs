using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Exceptions.Handler
{
    public class CustomExceptionHandler(ILogger<CustomExceptionHandler> logger)
        : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError($"Error Message:{exception.Message}, Time of currence {DateTime.UtcNow}");

            (string Detail, string Title, int statusCodes) details = exception switch
            {
                InternalServerException => (exception.Message, exception.GetType().Name, httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError),
                ValidationException => (exception.Message,exception.GetType().Name,httpContext.Response.StatusCode=StatusCodes.Status400BadRequest),
                BadRequestException=> (exception.Message, exception.GetType().Name, httpContext.Response.StatusCode = StatusCodes.Status400BadRequest),
                NotFoundException=> (exception.Message, exception.GetType().Name, httpContext.Response.StatusCode = StatusCodes.Status404NotFound),
                _=>(exception.Message, exception.GetType().Name, httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError)
            };

            var problemDetails = new ProblemDetails
            {
                Title = details.Title,
                Detail = details.Detail,
                Status = details.statusCodes,
                Instance = httpContext.Request.Path
            };

            problemDetails.Extensions.Add("traceId", httpContext.TraceIdentifier);

            if (exception is ValidationException validationException)
                problemDetails.Extensions.Add("validationErrors", validationException.Errors);


            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;


        }
    }
}
