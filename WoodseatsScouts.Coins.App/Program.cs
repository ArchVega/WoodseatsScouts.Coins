using Microsoft.EntityFrameworkCore;
using WoodseatsScouts.Coins.App.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appSettings.json")
    .AddJsonFile($"appSettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>((_, options) =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("WoodseatsScouts.Coins"));
});

// Add services to the container.

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();