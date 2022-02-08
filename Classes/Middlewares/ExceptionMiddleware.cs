using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace COT.API.Middlewares
{
    [ExcludeFromCodeCoverage]
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger _logger;
        public ExceptionMiddleware(RequestDelegate next,ILogger<ExceptionMiddleware> logger)
        {
            this.next = next;
            this._logger = logger;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                // log the exception to file or database here
                LogExceptionToFile(ex.Message,$"Exception-{DateTime.Now.ToBinary()}.txt"); // saving log to /Log folder

                _logger.LogError(ex, ex.Message);
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync(ex.Message);
            }
        }

        private void LogExceptionToFile(string message,string logFileName = "Log.txt")
        {
            string currentDirectory = Directory.GetCurrentDirectory();

            #region create log folder if not exists
            bool exists = Directory.Exists($"{currentDirectory}/Logs");
            if (!exists)
                System.IO.Directory.CreateDirectory($"{currentDirectory}/Logs");

            #endregion

            string filePath = $"{currentDirectory}/Logs/{logFileName}";

            using (StreamWriter w = File.AppendText(filePath))
            {
                w.WriteLine($"---------- {DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()} ------------");
                w.WriteLine($"Message: {message}");
                w.WriteLine("-----------------------------------------------");
            }
        }       
    }
}
