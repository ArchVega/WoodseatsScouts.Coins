namespace WoodseatsScouts.Coins.Api.Abstractions;

public interface IImagePersister
{
    FileInfo Persist(string jpegFileNameWithoutExtension, string base64PhotoData);
}