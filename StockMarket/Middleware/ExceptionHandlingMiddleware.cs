using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace StockMarket.Middleware
{
  // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
  public class ExceptionHandlingMiddleware
  {
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
      _next = next;
      _logger = logger;
    }

    public Task Invoke(HttpContext httpContext)
    {
      try
      {
        return _next(httpContext);
      }
      catch (Exception ex)
      {
        if(ex.InnerException != null)
        {
          _logger.LogError("{ErrorType} {ErrorMessage}", ex.InnerException.GetType().Name, ex.InnerException.Message);
        }
        else
        {
          _logger.LogError("{ErrorType} {ErrorMessage}", ex.GetType().Name, ex.Message);
        }
        throw;
      }
    }
  }

  // Extension method used to add the middleware to the HTTP request pipeline.
  public static class ExceptionHandlingMiddlewareExtensions
  {
    public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
    {
      return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
  }
}
