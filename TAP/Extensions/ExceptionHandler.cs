using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TAP.Data;
using TAP.Models;

namespace TAP.Extensions
{
    public class ExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<IClient> _logger;

        public ExceptionHandler(RequestDelegate next, ILogger<IClient> logger)
        {
            _logger = logger;
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

                _logger.LogError($"Something went wrong: {ex} ");
                await HandleExceptionAsync(httpContext);
            }
        }

        private Task HandleExceptionAsync(HttpContext httpContext)
        {


            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

            return httpContext.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = httpContext.Response.StatusCode,
                Message = "Unauthorized"
            }.ToString());
        }

       
    }
}
