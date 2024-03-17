// dotcover disable
using WoodseatsScouts.Coins.Api.Config;
using WoodseatsScouts.Coins.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appSettings.json")
    .AddJsonFile($"appSettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

const string allOrigins = "_allOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allOrigins, policy => { policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });
});

builder.Services.AddControllers();
ServicesRegistration.RegisterAll(builder.Services, builder.Configuration, builder.Environment);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Todo: consider uncommenting in staging / release
// if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "AcceptanceTest")
// {
    app.UseSwagger();
    app.UseSwaggerUI();
// }

// Todo: consider uncommenting in staging / release
// app.UseHttpsRedirection();
app.UseCors(allOrigins);
app.MapControllers();
app.UseMiddleware<ExceptionHandlingMiddleware>();

AppStartupValidator.Validate(app);

app.Run();