using System.Runtime.Serialization.Json;
using System.Threading;
using System.Net;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using API.Errors;
using System.Text.Json;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;    
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger,
       
        IHostEnvironment env)
                {
            _env = env;
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex,ex.Message);
                context.Response.ContentType ="application/json";
                context.Response.StatusCode=(int)HttpStatusCode.InternalServerError;

                var Response=_env.IsDevelopment()
                        ? new ApiException(context.Response.StatusCode,ex.Message,ex.StackTrace?.ToString())
                        : new ApiException(context.Response.StatusCode,"Internal server error");

                var options = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
                var Json = JsonSerializer.Serialize(Response,options);

                await context.Response.WriteAsync(Json);
                }
            }
        }
    }
