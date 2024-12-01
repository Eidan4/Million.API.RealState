using Million.API.RealEstate.Application.Exeptions;
using Newtonsoft.Json;
using System.Net;

namespace Intexus.IHI.Notifications.API.Middleware
{
    public class ExceptionMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            string result;

            switch (exception)
            {
                case BadRequestException badRequestException:
                    statusCode = HttpStatusCode.BadRequest;
                    result = JsonConvert.SerializeObject(new ErrorDetails
                    {
                        ErrorMessage = badRequestException.Message,
                        ErrorType = "BadRequest"
                    });
                    break;
                case ValidationsException validationException:
                    statusCode = HttpStatusCode.BadRequest;
                    result = JsonConvert.SerializeObject(new ErrorDetails
                    {
                        ErrorMessage = string.Join(", ", validationException.Errors),
                        ErrorType = "Validation"
                    });
                    break;
                case NotFoundException notFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    result = JsonConvert.SerializeObject(new ErrorDetails
                    {
                        ErrorMessage = notFoundException.Message,
                        ErrorType = "NotFound"
                    });
                    break;
                default:
                    result = JsonConvert.SerializeObject(new ErrorDetails
                    {
                        ErrorMessage = exception.Message,
                        ErrorType = "Failure"
                    });
                    break;
            }

            context.Response.StatusCode = (int)statusCode;
            return context.Response.WriteAsync(result);
        }
    }

    public class ErrorDetails
    {
        public string? ErrorType { get; set; }
        public string? ErrorMessage { get; set; }
    }
}