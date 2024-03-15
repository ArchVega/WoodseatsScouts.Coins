namespace WoodseatsScouts.Coins.Api.Abstractions;

public interface IImagePersister
{
    void Persist(string jpegFileNameWithoutExtension, string base64PhotoData);
}