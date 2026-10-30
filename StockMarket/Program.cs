using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using Services;
using StockMarket;
using Repositories;
using RepositroyContracts;
var builder = WebApplication.CreateBuilder(args);

#region Services
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IFinnhubService, FinnhubService>();
builder.Services.AddScoped<IStocksService, StocksService>();
builder.Services.AddScoped<IFinnhubRepository, FinnhubRepository>();
builder.Services.AddScoped<IStocksRepository, StocksRepository>();
builder.Services.AddHttpClient();
builder.Services.Configure<TradingOptions>(builder.Configuration.GetSection(nameof(TradingOptions)));
builder.Services.AddDbContext<StockMarketDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
#endregion

var app = builder.Build();


if(!app.Environment.IsEnvironment("Test"))
  Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");


app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();

public partial class Program { }