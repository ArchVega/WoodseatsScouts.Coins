using Microsoft.Extensions.Options;
using WoodseatsScouts.Coins.Api.Abstractions;
using WoodseatsScouts.Coins.Api.Config;

namespace WoodseatsScouts.Coins.Api.AppLogic;

public class ImagePersister(IOptions<AppSettings> appSettingsOptions, IHostEnvironment hostEnvironment) : IImagePersister
{
    private readonly AppSettings appSettings = appSettingsOptions.Value;

    public FileInfo Persist(string jpegFileNameWithoutExtension, string base64PhotoData)
    {
        var convert = base64PhotoData.Replace("data:image/jpeg;base64,", string.Empty);
        
        var imageDirectoryFullPath = Path.IsPathRooted(appSettings.ContentRootDirectory)
            ? appSettings.ContentRootDirectory
            : Path.Join(hostEnvironment.ContentRootPath, appSettings.ContentRootDirectory);

        if (!Path.Exists(imageDirectoryFullPath))
        {
            throw new ApplicationException($"Image directory does not exist at path '{imageDirectoryFullPath}'");
        }
        
        var contentRootDirectoryInfo = new DirectoryInfo(appSettings.ContentRootDirectory);
        var directoryInfo = new DirectoryInfo(imageDirectoryFullPath);

        var imageFileName = $"{jpegFileNameWithoutExtension}.jpg";
        var imageFullPath = Path.Join(imageDirectoryFullPath, imageFileName);
        var imageFileInfo = new FileInfo(imageFullPath);
        
        File.WriteAllBytes(imageFullPath, Convert.FromBase64String(convert));

        // dotcover disable
        if (!imageFileInfo.Exists)
        {
            throw new ApplicationException($"Image was not created at path '{imageFileInfo.FullName}'");
        }
        // dotcover enable

        return imageFileInfo;
    }

    public byte[] RetrieveImageBytes(int id)
    {
        var imageDirectoryFullPath = Path.IsPathRooted(appSettings.ContentRootDirectory)
            ? appSettings.ContentRootDirectory
            : Path.Join(hostEnvironment.ContentRootPath, appSettings.ContentRootDirectory);

        if (!Path.Exists(imageDirectoryFullPath))
        {
            throw new ApplicationException($"Image directory does not exist at path '{imageDirectoryFullPath}'");
        }

        var path = Path.Combine(imageDirectoryFullPath, $"{id}.jpg");
        
        if (!Path.Exists(path))
        {
            throw new ApplicationException($"Image file does not exist at path '{path}'");
        }

        return File.ReadAllBytes(path);
    }
}