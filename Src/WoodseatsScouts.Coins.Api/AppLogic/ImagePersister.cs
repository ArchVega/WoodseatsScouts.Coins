using Microsoft.Extensions.Options;
using WoodseatsScouts.Coins.Api.Abstractions;
using WoodseatsScouts.Coins.Api.Config;

namespace WoodseatsScouts.Coins.Api.AppLogic;

public class ImagePersister(IOptions<AppSettings> appSettingsOptions, IHostEnvironment hostEnvironment) : IImagePersister
{
    private readonly AppSettings appSettings = appSettingsOptions.Value;

    public void Persist(string jpegFileNameWithoutExtension, string base64PhotoData)
    {
        var convert = base64PhotoData.Replace("data:image/jpeg;base64,", string.Empty);
        var rootPath =
            appSettings.ContentRootDirectory
            ?? Path.Join(hostEnvironment.ContentRootPath, "..", "woodseatsscouts.coins.web", "public", "member-images");
        var photoFileName = $"{jpegFileNameWithoutExtension}.jpg";
        var photoFullPath = Path.Join(rootPath, photoFileName);
        File.WriteAllBytes(photoFullPath, Convert.FromBase64String(convert));
    }
}