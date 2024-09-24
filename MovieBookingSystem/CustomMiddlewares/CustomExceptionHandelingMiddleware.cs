using System.Net;

namespace MovieBookingSystem.CustomMiddlewares
{
    public class CustomExceptionHandelingMiddleware
    {
        private readonly RequestDelegate _next;
        public CustomExceptionHandelingMiddleware(RequestDelegate next) { 
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                 await _next(context);
            }
            catch (Exception e) {
                await HandleExcdeption(context, e);
            }
        }

        private Task HandleExcdeption(HttpContext context, Exception e)
        {
            var response = new
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Message = "an unexcpected error has occurred!. Please try again.",
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return context.Response.WriteAsJsonAsync(response);
        }
    }
}
