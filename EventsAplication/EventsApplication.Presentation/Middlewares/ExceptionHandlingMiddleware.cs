using EventsApplication.Domain.Exceptions;

namespace EventsApplication.Presentation.Meddlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NotFoundException exception)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await HandleExceptionAsync(context, exception);
            }
            catch (BadRequestException exception)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await HandleExceptionAsync(context, exception);
            }
            catch (Exception exception)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await HandleExceptionAsync(context, exception);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError("The problem occured: {Exception}", exception.Message);
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync($"{exception.Message}\n{context.Response.StatusCode}");
        }
    }
}
