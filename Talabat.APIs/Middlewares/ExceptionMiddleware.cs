﻿using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using Talabat.APIs.Errors;

namespace Talabat.APIs.Middleware
{
    //1.Convention Based

    //2.Factory Based [ : IMiddleware]
    public class ExceptionMiddleware :IMiddleware
    {
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }
        

        public async Task InvokeAsync(HttpContext httpContext, RequestDelegate _next)
        {       //take an action with the request
            try
            {   
                await _next.Invoke(httpContext); 
                // Go to next middleware
                //take an action with the response
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message); // Development env

                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                httpContext.Response.ContentType = "application/json";

                var response = _env.IsDevelopment() ? new ApiExceptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString()) :
                
                new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);

                var options = new JsonSerializerOptions() 
                { 
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                var json = JsonSerializer.Serialize(response, options);
                await httpContext.Response.WriteAsync(json);
            }

        }
    }
}
