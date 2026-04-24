// dotcover disable

using System.Reflection;
using Microsoft.OpenApi.Models;
using WoodseatsScouts.Coins.Api.Abstractions;
using WoodseatsScouts.Coins.Api.Config;
using WoodseatsScouts.Coins.Api.Middleware;
using WoodseatsScouts.Coins.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

const string allOrigins = "_allOrigins";
builder.Services.AddCors(options => { options.AddPolicy(name: allOrigins, policy => { policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); }); });

builder.Services.AddScoped<IMemberService, MemberService>();

builder.Services.AddControllers();
ServicesRegistration.RegisterAll(builder.Services, builder.Configuration, builder.Environment);
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

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

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

// var cultureInfo = new CultureInfo("en-GB");
//
// // Sets the culture for the current thread
// CultureInfo.CurrentCulture = cultureInfo;
// CultureInfo.CurrentUICulture = cultureInfo;
//
// // Sets the default culture for all new threads in the app
// CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
// CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
//
// var supportedCultures = new[] { "en-GB" };
// var localizationOptions = new RequestLocalizationOptions()
//     .SetDefaultCulture(supportedCultures[0])
//     .AddSupportedCultures(supportedCultures)
//     .AddSupportedUICultures(supportedCultures);
//
// app.UseRequestLocalization(localizationOptions);

app.Run();