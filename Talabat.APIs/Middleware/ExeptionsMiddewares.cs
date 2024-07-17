using System.Net;
using System.Text.Json;
using Talabat.APIs.Errors;

namespace Talabat.APIs.Middleware
{
    public class ExeptionsMiddewares
    {
        public RequestDelegate _next;
        public ILogger<ExeptionsMiddewares> _logger;
        public IHostEnvironment _env;

        public ExeptionsMiddewares(RequestDelegate next, ILogger<ExeptionsMiddewares> logger , IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync (HttpContext context)
        {
            try {
                await _next.Invoke(context);
             }catch (Exception ex){
                _logger.LogError(ex,ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var response = _env.IsDevelopment()? new  ApiExeptionResponse(500,ex.Message, ex.StackTrace.ToString()):
                    new  ApiExeptionResponse(500);
                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
        }


    }
}
