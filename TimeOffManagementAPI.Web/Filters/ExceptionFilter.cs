using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TimeOffManagementAPI.Web.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var response = context.HttpContext.Response;
        response.ContentType = "application/json";

        switch (context.Exception)
        {
            case KeyNotFoundException:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                break;
            case FormatException:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                break;
            case UnauthorizedAccessException:
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
                break;
            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
        }

        var result = JsonSerializer.Serialize(new
        {
            error = context.Exception.Message,
            stackTrace = context.Exception.StackTrace
        });

        response.WriteAsync(result);
    }
}