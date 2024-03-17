using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Options;
using Shouldly;
using WoodseatsScouts.Coins.Api.AppLogic;
using WoodseatsScouts.Coins.Api.Config;

// ReSharper disable InconsistentNaming

namespace WoodseatsScouts.Coins.Tests.Integration.Images;

public class ImagePersisterTests
{
    private const string TestImageBlackRectangle =
        "iVBORw0KGgoAAAANSUhEUgAAA+gAAAPoCAMAAAB6fSTWAAAABGdBTUEAALGPC/xhBQAAAAFzUkdCAK7OHOkAAABmUExURUdwTAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAItlF7YAAAAhdFJOUwDl9o0FlcP6IZSQjuwHyrfyxcKjjyPEGSKTAqIDGuuR7VueEjIAAAotSURBVHja7NbbCYRAEEVBRde3iKDL+iOYf5JG0dsKVSHMPQ1TFAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAALxO1zflBQHK5uhc2CPUn1aPxGn72pXlmxcpEmuZ3Vm6UYdEq9xZtt2/nfjf+8+lJRtUSLzNpSVbRUi8r0tLNomQeKdLS6ZB/sGlOXQcOg4dh45Dx6Hj0HHoOHQcukMHh+7QwaG/9dC9CLoyCOjKIKArg6ArDIKuMAi6wiDoCoOgK4MYBF0ZBHRlENCVQdCVrgyCrjAIusIg6AqDoCsMgq4MAroyCOjKIOhKVwZBVxgEXWEQdIVB0BUGQVcGAV0ZBHRlENCVQdAVBkFXGARdYRB0hUHQlUEMgq4MAroyCOjKIOjKixgEXWEQdIVB0BUGQVcYBF0ZBHRlENCVQdCVrgyCrjAIusIg6AqDoCsMgq4MAroyCOjKIKArg6ArDIKuMAi6wiDoCoOgK4MYBF0ZBHRlENCVQdAVBkFXGARdYRB0hUHQFQZBVwYBXRkEdGUQdKUrg6ArDIKuMAi6wiDoCoOgK4OArgwCujII6Mog6AqDoCsMgq4wCLrCIOjKIAZBVwYBXRkEdGUQdIVB0BUGQVcYBF1hEHSFQdCVQUBXBgFdGQRd6cog6AqDoCsMgq4wCLrCIOjKIKArg4CuDAK6Mgi6wiDoCoOgKwyCrjAIujKIQdCVQUBXBgFdGQRdYRB0hUHQFQZBVxgEXWEQdGUQ0JVBQFcGQVe6Mgi6wiDoCoOgKwyCrjAIujII6MogoCuDgK4Mgq4wCLrCIOgKg6ArDIKuDGIQdGUQ0JVBQFcGQVcYBF1hEHSFQdAVBkFXBjEIujII6MogoCuDoCtdGQRdYRB0hUHQFQZBVxgEXRkEdGUQ0JVBQFcGQVcYBF1hEHSFQdAVBkFXBjEIujII6MogoCuDoCsMgq4wCLrCIOgKg6ArgxgEXRkEdGUQ0JVB0JWuDIKuMAi6wiDoCoOgKwyCrgwCujII6Mog6EpXBkFXGARdYRB0hUHQFQZBVwYxCLoyCOjKIKArg6ArDIKuMAi6wiDoCoOgK4MYBF0ZBHRlENCVQdCVrgyCrjAIusIg6AqDoCsMgq4MAroyCOjKIOhKVwZBVxgEXWEQdIVB0BUGQVcGMQi6MgjoyiCgK4OgKwyCrjAIusIg6AqDoCuDGARdGQR0ZRDQlUHQla4Mgq4wCLrCIOgKg6ArDIKuDAK6MgjoyiDoSlcGQVcYBF1hEHSFQdAVBkFXBjEIujII6MogoCuDoCsMgq4wCLrCIOgKg6ArgxgEXRkEdGUQ0JVB0JWuDIKuMAi6wiDoCoOgKwyCrgwCujII6Mog6EpXBkFXGARdYRB0hUHQFQZBVwbxIujKIKArg4CuDIKuMAi6wiDoCoOgKwyCrgxiEHRlENCVQUBXBkFXujIIusIg6AqDoCsMgq4wCLoyCOjKIKArg6ArXRkEXWEQdIVB0BUGQVcYBF0ZBHRlENCVQUBXBkFXGARdYRB0hUHQFQZBVwYxCLoyCOjKIKArg6ArL2IQdIVB0BUGQVcYBF1hEHRlENCVQUBXBkFXujIIusIg6AqDoCsMgq4wCLoyCOjqbp/OUhAGgCAKQvYYCUE/Ai6Y+5/SUzRjpOoI028MAroyCOjKIOgKg6ArDIKuMAi6wiDoyiAGQVcGAV0ZBHRlEHSFQdAVBkFXGARdYRB0hUHQlUFAVwYBXRkEXenKIOgKg6ArDIKuMAi6wiDoyiCgK4OArgwCujIIusIg6AqDoCsMgq4wCLoyiEHQlUFAVwYBXRkEXWEQdIVB0BUGQVcYBF1hEHRlENCVQUBXBkFXujIIusIg6AqDoCsMgq4wCLoyCOjKIKArg4CuDIKuMAi6wiDoCoOgKwyCrgxiEHRlENCVQUBXBkFXGARdYRB0hUHQFQZBVxgEXRkEdGUQ0JVB0JWuDIKuMAi6wiDoCoOgKwyCrgwCujII6MogoCuDoCsMgq4wCLrCIOgKg6ArgxgEXRkEdGUQ0JVB0BUGQVcYBF1hEHSFQdCVQQyCrgwCujII6Mog6EpXBkFXGARdYRB0hUHQFQZBVwYBXRkEdGUQ0JVB0BUGQVcYBF1hEHSFQdCVQQyCrgwCujII6Mog6AqDoCsMgq4wCLrCIOjKIAZBVwYBXRkEdGUQdKUrg6ArDIKuMAi6wiDoCoOgK4OArgwCujIIutKVQdAVBkFXGARdYRB0hUHQlUEMgq4MAroyCOjKIOgKg6ArDIKuMAi6wiDoyiAGQVcGAV0ZBHRlEHSlK4OgKwyCrjAIusIg6AqDoCuDgK4MAroyCLrSlUHQFQZBVxgEXWEQdIVB0JVBDIKuDAK6MgjoyiDoCoOgKwyCrjAIusIg6MogBkFXBgFdGQR0ZRB0pSuDoCsMgq4wCLrCIOgKg6Arg4CuDAK6Mgi60pVB0BUGQVcYBF1hEHSFQdCVQQyCrgwCujII6Mog6AqDoCsMgq4wCLrCIOjKIAZBVwYBXRkEdGUQdKUrg6ArDIKuMAi6wiDoCoOgK4OArgwCujIIutKVQdAVBkFXGARdYRB0hUHQlUFcBF0ZBHRlENCVQdAVBkFXGARdYRB0hUHQlUEMgq4MAroyCOjKIOhKVwZBVxgEXWEQdIVB0BUGQVcGAV0ZBHRlEHSlK4OgKwyCrjAIusIg6AqDoCuDgK4MAroyCOjKIOgKg6ArDIKuMAi6wiDoyiAGQVcGAV0ZBHRlEHTlIgZBVxgEXWEQdIVB0BUGQVcGAV0ZBHRlEHSlK4OgKwyCrjAIusIg6AqDoCuDgK4MAroyCOjKIOgKg6ArDIKuMAi6wiDoyiAGQVcGAV0ZBHRlEHSFQdAVBkFXGARdYRB0hUHQlUFAV/85CCT4NI+OR8ej49Hx6Hh0PDoeHY+OR/fo4NE9Onj0szg0SN7VpxUbREjezacV+4iQvItPK/ZuVEhas/u0aosMSWv9WbnXXYdkPR/+rF7Xj1IkZ5w7X/YTpn7d9EjCts6TDwMAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAADifL4EVTisEv7+DAAAAAElFTkSuQmCC";

    [Fact]
    public void FullPath_CanSaveImageToDisk()
    {
        const string appSettings_ContentRootDirectory = "TestImage";
        const string hostingEnvironment_ContentRoot = "";

        var (appSettingsOptions, hostingEnvironment) = Setup(appSettings_ContentRootDirectory, hostingEnvironment_ContentRoot, true);

        var imagePersister = new ImagePersister(appSettingsOptions, hostingEnvironment);

        var savedImageFileInfo = Should.NotThrow(() => imagePersister.Persist("test", TestImageBlackRectangle));

        savedImageFileInfo.Length.ShouldBePositive();
    }


    [Fact]
    public void PartialPath_CanSaveImageToDisk()
    {
        const string appSettings_ContentRootDirectory = "TestImage";
        var hostingEnvironment_ContentRoot = Directory.GetCurrentDirectory();

        var (appSettingsOptions, hostingEnvironment) = Setup(appSettings_ContentRootDirectory, hostingEnvironment_ContentRoot, true);

        var imagePersister = new ImagePersister(appSettingsOptions, hostingEnvironment);

        var savedImageFileInfo = Should.NotThrow(() => imagePersister.Persist("test", TestImageBlackRectangle));

        savedImageFileInfo.Length.ShouldBePositive();
    }

    [Fact]
    public void IncorrectFullPath_ThrowsException()
    {
        const string appSettings_ContentRootDirectory = "Y:\\does-not-exist";
        const string hostingEnvironment_ContentRoot = "";

        var (appSettingsOptions, hostingEnvironment) = Setup(appSettings_ContentRootDirectory, hostingEnvironment_ContentRoot, false);

        var imagePersister = new ImagePersister(appSettingsOptions, hostingEnvironment);

        var exception = Should.Throw<ApplicationException>(() => imagePersister.Persist("test.jpg", TestImageBlackRectangle));

        exception.Message.ShouldStartWith("Image directory does not exist at path");
    }

    [Fact]
    public void IncorrectRelativePaths_ThrowsException()
    {
        const string appSettings_ContentRootDirectory = ".\\assumed-valid-relative-path";
        const string hostingEnvironment_ContentRoot = "Y:\\nonexistent-content-root";

        var (appSettingsOptions, hostingEnvironment) = Setup(appSettings_ContentRootDirectory, hostingEnvironment_ContentRoot, false);

        var imagePersister = new ImagePersister(appSettingsOptions, hostingEnvironment);

        var exception = Should.Throw<ApplicationException>(() => imagePersister.Persist("test.jpg", TestImageBlackRectangle));

        exception.Message.ShouldStartWith("Image directory does not exist at path");
    }

    private static (IOptions<AppSettings> appSettingsOptions, HostingEnvironment hostingEnvironment) Setup(
        string appSettingsContentRootDirectory,
        string hostingEnvironmentContentRoot,
        bool createTestDirectory)
    {
        var directoryInfo = new DirectoryInfo(appSettingsContentRootDirectory);

        if (directoryInfo.Exists)
        {
            directoryInfo.Delete(true);
        }

        if (createTestDirectory)
        {
            directoryInfo.Create();
        }

        var appSettings = new AppSettings
        {
            ContentRootDirectory = appSettingsContentRootDirectory // Note
        };

        var appSettingsOptions = Options.Create(appSettings);

        var hostingEnvironment = new HostingEnvironment
        {
            ContentRootPath = hostingEnvironmentContentRoot // Note
        };

        return (appSettingsOptions, hostingEnvironment);
    }
}