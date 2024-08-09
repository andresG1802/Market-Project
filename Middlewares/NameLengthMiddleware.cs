// Middlewares/NameLengthMiddleware.cs
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace MiApi.Middlewares
{
    public class NameLengthMiddleware
    {
        private readonly RequestDelegate _next;

        public NameLengthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Method == HttpMethods.Post && context.Request.ContentType == "application/json")
            {
                context.Request.EnableBuffering();

                using (var reader = new StreamReader(context.Request.Body, leaveOpen: true))
                {
                    var body = await reader.ReadToEndAsync();
                    context.Request.Body.Position = 0;

                    if (!string.IsNullOrEmpty(body))
                    {
                        var jsonDoc = JsonDocument.Parse(body);
                        if (jsonDoc.RootElement.TryGetProperty("name", out JsonElement nameElement))
                        {
                            var name = nameElement.GetString();
                            if (name != null && name.Length <= 8)
                            {
                                context.Response.StatusCode = 400; // Bad Request
                                await context.Response.WriteAsync("El nombre debe tener más de 4 caracteres.");
                                return;
                            }
                        }
                    }
                }
            }

            await _next(context);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class NameLengthMiddlewareExtensions
    {
        public static IApplicationBuilder UseNameLengthMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<NameLengthMiddleware>();
        }
    }
}

