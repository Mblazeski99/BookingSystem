using BookingSystem.Domain.Models.Exceptions;
using BookingSystem.Models;
using System.Net;

namespace BookingSystem.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

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

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            string message = "Internal Server Error";

            if (exception is CustomDataException)
            {
                var customException = (exception as CustomDataException);

                if (string.IsNullOrEmpty(customException?.CustomErrorMessage) == false)
                {
                    message = customException.CustomErrorMessage;
                }
            }

            var error = new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = message
            };

            await context.Response.WriteAsync(message);
        }
    }
}