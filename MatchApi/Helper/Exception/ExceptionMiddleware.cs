using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;

namespace MatchApi.Helper
{
    public class ExceptionMiddleware
    {
        private RequestDelegate Request { get; }
        public IHostingEnvironment Environment { get; }
        public ILogger Logger { get; }

        public ExceptionMiddleware(RequestDelegate request, IHostingEnvironment environment, ILogger logger)
        {
            this.Request = request;
            this.Environment = environment;
            this.Logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await Request(context).ConfigureAwait(false);
            }
            //catch (DomainException exception)
            //{
            //    await ResponseAsync(context, HttpStatusCode.UnprocessableEntity, exception.Message).ConfigureAwait(false);
            //}
            catch (Exception exception)
            {
                if (Environment.IsDevelopment())
                {
                    throw;
                }
                Logger.LogError(exception.ToString());
                await ResponseAsync(context, HttpStatusCode.InternalServerError, string.Empty).ConfigureAwait(false);
            }
        }

        private static async Task ResponseAsync(HttpContext context, HttpStatusCode statusCode, string response)
        {
            context.Response.Clear();
            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = MediaTypeNames.Application.Json;
            await context.Response.WriteAsync(response).ConfigureAwait(false);
        }
    }
}
