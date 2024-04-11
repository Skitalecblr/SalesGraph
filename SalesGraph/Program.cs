using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SalesGraph.Core.Configuration;
using SalesGraph.Core.DataAccess.Context;
using SalesGraph.Core.Services.Implementation;
using SalesGraph.Core.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var dbSettings = new DbSettings();
builder.Configuration.GetSection("DB").Bind(dbSettings);

builder.Services.Configure<DbSettings>(options => { builder.Configuration.GetSection("DB").Bind(options); });

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<SalesGraphContext>(options =>
    options.UseSqlServer(dbSettings.ConnectionStrings.Api));

builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<ISalesService, SalesService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{ 
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();
