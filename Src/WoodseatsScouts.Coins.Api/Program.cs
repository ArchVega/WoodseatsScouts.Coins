// dotcover disable

using Microsoft.OpenApi.Models;
using WoodseatsScouts.Coins.Api.Config;
using WoodseatsScouts.Coins.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

const string allOrigins = "_allOrigins";
ServicesRegistration.RegisterAll(builder.Services, builder.Configuration, builder.Environment);
builder.Services.AddCors(options => { options.AddPolicy(name: allOrigins, policy => { policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); }); });
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition(AppSettings.ApiAuthenticationTokenKey, new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.ApiKey,
        Name = AppSettings.ApiAuthenticationTokenKey,
        In = ParameterLocation.Header,
        Description = "Enter your authentication token for the Scouts Coin API service"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Name = AppSettings.ApiAuthenticationTokenKey,
                Type = SecuritySchemeType.ApiKey,
                In = ParameterLocation.Header,
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = AppSettings.ApiAuthenticationTokenKey
                }
            },
            []
        }
    });

    // todo
    // var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    // var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    // c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors(allOrigins);
app.MapControllers();
app.UseMiddleware<ExceptionHandlingMiddleware>();
AppStartupValidator.Validate(app);
app.Run();