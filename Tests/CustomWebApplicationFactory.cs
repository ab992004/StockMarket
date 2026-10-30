using Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;

namespace Tests
{
  public class CustomWebApplicationFactory : WebApplicationFactory<Program>
  {
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
      base.ConfigureWebHost(builder);

      builder.UseEnvironment("Test");

      builder.ConfigureServices(services => {

        services.RemoveAll<DbContextOptions<StockMarketDbContext>>();
        services.RemoveAll<IDbContextOptionsConfiguration<StockMarketDbContext>>();

        services.AddDbContext<StockMarketDbContext>(options =>
        {
          options.UseInMemoryDatabase("DatbaseForTesting");
        });
      });
    }
  }
}