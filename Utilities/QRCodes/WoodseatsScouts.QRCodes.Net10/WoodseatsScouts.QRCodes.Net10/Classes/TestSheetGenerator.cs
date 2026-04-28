using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using WoodseatsScouts.QRCodes.Classes;

namespace WoodseatsScouts.QRCodes.Net10.Classes;

public class TestSheetGenerator(DirectoryInfo databaseDirectoryInfo, DirectoryInfo testSheetsDirectoryInfo, DirectoryInfo testSheetsPaddedDirectoryInfo)
{
    private static readonly int Seed;

    private readonly Random random = new(Seed);

    static TestSheetGenerator()
    {
        Seed = 1024;
    }

    public void Generate(List<ScoutMember> members, List<Coin> coins)
    {
        var uniqueRandoms = new UniqueRandoms(random);

        var s = new List<int> { 3, 5, 7, 9 };

        var specialCoinsStack = new Stack<Coin>(coins.Where(x => s.Contains(x.Value)));

        var coinIndexesStack = new Stack<int>(coins.Select((x, y) => y));

        foreach (var member in members)
        {
            var fileInfo = databaseDirectoryInfo.GetFiles($"*{member.FullName}*", SearchOption.AllDirectories).Single();
            var memberQrCodeImageFileInfo = new MemberFileInfo
            {
                ScoutMember = member,
                FileInfo = fileInfo,
                Bitmap = Image.Load<Rgba32>(fileInfo.FullName)
            };

            var randomIndexes = uniqueRandoms.Get(4, coinIndexesStack.Count);
            var randomCoinIndexStack = new Stack<int>(randomIndexes);

            var randomCoins = new List<Coin>();
            while (randomCoinIndexStack.Count > 0)
            {
                randomCoins.Add(coins[randomCoinIndexStack.Pop()]);
            }

            var coinQrCodeImageFileInfos = randomCoins
                .Select(x =>
                {
                    var qrCodeFileInfo = databaseDirectoryInfo.GetFiles($"*{x}*", SearchOption.AllDirectories).Single();
                    return new CoinFileInfo()
                    {
                        Coin = x,
                        FileInfo = qrCodeFileInfo,
                        Bitmap = Image.Load<Rgba32>(qrCodeFileInfo.FullName)
                    };
                }).ToList();

            var specialCoin = specialCoinsStack.Pop();
            var qrCodeFileInfo = databaseDirectoryInfo.GetFiles($"*{specialCoin}*", SearchOption.AllDirectories)
                .Single();
            coinQrCodeImageFileInfos.Add(new CoinFileInfo()
            {
                Coin = specialCoin,
                FileInfo = qrCodeFileInfo,
                Bitmap = Image.Load<Rgba32>(qrCodeFileInfo.FullName)
            });

            GenerateTestSheetImage(memberQrCodeImageFileInfo, coinQrCodeImageFileInfos);
        }
    }

    private void GenerateTestSheetImage(MemberFileInfo memberQrCodeImageFileInfo, List<CoinFileInfo> coinQrCodeImageFileInfos)
    {
        const int outputImageWidth = 1920; // memberImage.Width > secondImage.Width ? memberImage.Width : secondImage.Width;
        const int outputImageHeight = 1200; //memberImage.Height + secondImage.Height + 1;

        // var outputImage = new Bitmap(outputImageWidth, outputImageHeight, PixelFormat.Format32bppArgb);
        var outputImage = new Image<Rgba32>(outputImageWidth, outputImageHeight);

        outputImage.Mutate(ctx =>
        {
            // Fill background white
            ctx.Fill(Color.White);

            // Setup font (you can pick a font installed on Linux)
            var collection = new FontCollection();
            var fontFamily = collection.Add("/usr/share/fonts/truetype/dejavu/DejaVuSans.ttf"); //
            var font = fontFamily.CreateFont(16);

            int row1X = 0;

            // Row 1
            ctx.DrawImage(memberQrCodeImageFileInfo.Bitmap, new Point(row1X, 0), 1f);
            DrawText(ctx, "MEMBER QR CODE", row1X, 0, font);
            DrawText(ctx, $"Code: {memberQrCodeImageFileInfo.ScoutMember.Code}", row1X, 22, font);
            DrawText(ctx, $"Member: {memberQrCodeImageFileInfo.ScoutMember.FullName}", row1X, 44, font);
            row1X += memberQrCodeImageFileInfo.Bitmap.Width;

            foreach (var coinImage in coinQrCodeImageFileInfos.Take(2))
            {
                ctx.DrawImage(coinImage.Bitmap, new Point(row1X, 0), 1f);
                DrawText(ctx, "COIN QR CODE", row1X, 0, font);
                DrawText(ctx, $"Code: {coinImage.Coin}", row1X, 22, font);
                DrawText(ctx, $"Value: {coinImage.Coin.Value}", row1X, 44, font);
                row1X += coinImage.Bitmap.Width;
            }

            // Row 2
            int row2X = 0;
            int row2Y = memberQrCodeImageFileInfo.Bitmap.Height + 1;

            foreach (var coinImage in coinQrCodeImageFileInfos.Skip(2).Take(3))
            {
                ctx.DrawImage(coinImage.Bitmap, new Point(row2X, row2Y), 1f);
                DrawText(ctx, "COIN QR CODE", row2X, row2Y, font);
                DrawText(ctx, $"Code: {coinImage.Coin}", row2X, 22 + row2Y, font);
                DrawText(ctx, $"Value: {coinImage.Coin.Value}", row2X, 44 + row2Y, font);
                row2X += coinImage.Bitmap.Width;
            }
        });

        var path = Path.Combine(testSheetsDirectoryInfo.FullName, $"{memberQrCodeImageFileInfo.ScoutMember.FullName}_TestSheet.png");

        outputImage.Save(path);
        
        ResizeWithPadding(outputImage, testSheetsPaddedDirectoryInfo, memberQrCodeImageFileInfo);
    }

    static void ResizeWithPadding(Image<Rgba32> original, DirectoryInfo testSheetsPaddedDirectoryInfo, MemberFileInfo memberQrCodeImageFileInfo)
    {
        // Set padding
        int padding = 40;

        // Create new image with extra padding on all sides
        int newWidth = original.Width + 2 * padding;
        int newHeight = original.Height + 2 * padding;

        using Image<Rgba32> padded = new Image<Rgba32>(newWidth, newHeight);

        // Fill background (optional)
        padded.Mutate(ctx => ctx.Fill(Color.White));

        // Draw original image onto new canvas at offset (padding, padding)
        padded.Mutate(ctx => ctx.DrawImage(original, new Point(padding, padding), 1f));

        var path = Path.Combine(testSheetsPaddedDirectoryInfo.FullName, $"{memberQrCodeImageFileInfo.ScoutMember.FullName}_TestSheet.png");
        Console.WriteLine($"Saving {path}");
        padded.Save(path);
    }

    static void DrawText(IImageProcessingContext ctx, string text, int x, int y, Font font)
    {
        ctx.DrawText(text, font, Color.Black, new PointF(x, y));
    }
}

public class MemberFileInfo
{
    public ScoutMember ScoutMember { get; set; }
    public FileInfo FileInfo { get; set; }
    public new Image<Rgba32> Bitmap { get; set; }
}

public class CoinFileInfo
{
    public Coin Coin { get; set; }
    public FileInfo FileInfo { get; set; }
    public new Image<Rgba32> Bitmap { get; set; }
}