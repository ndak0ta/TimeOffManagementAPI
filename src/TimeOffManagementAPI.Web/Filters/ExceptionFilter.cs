using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Filters;
using TimeOffManagementAPI.Exceptions;

namespace TimeOffManagementAPI.Web.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var response = context.HttpContext.Response;
        response.ContentType = "application/json";

        response.StatusCode = context.Exception switch
        {
            NotFoundException => (int)HttpStatusCode.NotFound,
            ArgumentNullException => (int)HttpStatusCode.BadRequest,
            DuplicateRecordException => (int)HttpStatusCode.Conflict,
            UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
            UnprocessableEntityException => (int)HttpStatusCode.UnprocessableEntity,
            _ => (int)HttpStatusCode.InternalServerError,
        };
        
        var result = JsonSerializer.Serialize(new
        {
            error = context.Exception.Message,
            stackTrace = context.Exception.StackTrace,
            innerException = context.Exception.InnerException?.Message
        });

        response.WriteAsync(result);
    }
}