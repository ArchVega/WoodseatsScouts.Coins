// dotcover disable

using Microsoft.Extensions.Options;

namespace WoodseatsScouts.Coins.Api.Config;

public static class AppStartupValidator
{
    public static void Validate(WebApplication app)
    {
        var appSettings = app.Services.GetService<IOptions<AppSettings>>()!.Value;
        if (string.IsNullOrWhiteSpace(appSettings?.ContentRootDirectory))
        {
            throw new ApplicationException("AppSettings.Json error: ContentRootDirectory is null or empty");
        }
        
        var imageDirectoryFullPath = Path.IsPathRooted(appSettings.ContentRootDirectory)
            ? appSettings.ContentRootDirectory
            : Path.Join(app.Environment.ContentRootPath, appSettings.ContentRootDirectory);

        var directory = new DirectoryInfo(imageDirectoryFullPath);

        if (!directory.Exists)
        {
            Directory.CreateDirectory(directory.FullName);
        }
    }
}