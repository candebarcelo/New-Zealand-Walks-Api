using System.Net;

namespace NZWalks.API.Middlewares
{
    // this is for global exception handling, i.e. it handles exceptions for all controllers and methods.
    public class ExceptionHandlerMiddleware
    {
        private readonly ILogger<ExceptionHandlerMiddleware> logger;
        private readonly RequestDelegate next;

        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger, 
            RequestDelegate next)
        {
            this.logger = logger;
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                // the request delegate will run the next requested method.
                await next(httpContext);
            }
            catch (Exception ex)
            {
                var errorId = Guid.NewGuid();

                // log the exception in the Console.
                logger.LogError(ex, $"{errorId}: {ex.Message}");

                // return custom error response
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";

                var error = new
                {
                    Id = errorId,
                    ErrorMessage = "Something went wrong! We are looking into resolving this."
                };
                // the error will be displayed in the response body.
                await httpContext.Response.WriteAsJsonAsync(error);
            }
        }
    }
}
