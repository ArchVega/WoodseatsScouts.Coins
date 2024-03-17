using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace WoodseatsScouts.QRCodes.Classes;

public class TestSheetGenerator(DirectoryInfo databaseDirectoryInfo, DirectoryInfo testSheetsDirectoryInfo)
{
    private static readonly int Seed;

    private Random random = new Random(Seed);

    static TestSheetGenerator()
    {
        Seed = 1024;
    }

    public void Generate(List<Member> members, List<Coin> coins)
    {
        var uniqueRandoms = new UniqueRandoms(random);

        var memberCodeStack = new Stack<Member>(members);
        var coinIndexesStack = new Stack<int>(coins.Select((x, y) => y));

        while (memberCodeStack.Count > 0)
        {
            var member = memberCodeStack.Pop();
        }

        foreach (var member in members)
        {
            var fileInfo = databaseDirectoryInfo.GetFiles($"*{member.FullName}*", SearchOption.AllDirectories).Single();
            var memberQrCodeImageFileInfo = new MemberFileInfo
            {
                Member = member,
                FileInfo = fileInfo,
                Bitmap = new Bitmap(fileInfo.FullName)

            };

            const int max = 5;
            var randomIndexes = uniqueRandoms.Get(5, coinIndexesStack.Count);
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
                        Bitmap = new Bitmap(qrCodeFileInfo.FullName)
                    };
                }).ToList();

            GenerateTestSheetImage(memberQrCodeImageFileInfo, coinQrCodeImageFileInfos);
        }
    }

    private void GenerateTestSheetImage(MemberFileInfo memberQrCodeImageFileInfo, List<CoinFileInfo> coinQrCodeImageFileInfos)
    {
        const int outputImageWidth = 1920; // memberImage.Width > secondImage.Width ? memberImage.Width : secondImage.Width;
        const int outputImageHeight = 1200; //memberImage.Height + secondImage.Height + 1;

        var outputImage = new Bitmap(outputImageWidth, outputImageHeight, PixelFormat.Format32bppArgb);

        using (var graphics = Graphics.FromImage(outputImage))
        {
            var row1X = 0;

            // Row 1
            graphics.DrawImage(
                memberQrCodeImageFileInfo.Bitmap,
                new Rectangle(new Point(), memberQrCodeImageFileInfo.Bitmap.Size),
                new Rectangle(new Point(), memberQrCodeImageFileInfo.Bitmap.Size),
                GraphicsUnit.Pixel);
            DrawText(graphics, "MEMBER QR CODE", row1X, 0);
            DrawText(graphics, $"Code: {memberQrCodeImageFileInfo.Member.Code}", row1X, 22);
            DrawText(graphics, $"Member: {memberQrCodeImageFileInfo.Member.FullName}", row1X, 44);
            row1X += memberQrCodeImageFileInfo.Bitmap.Width;

            foreach (var coinImage in coinQrCodeImageFileInfos.Take(2))
            {
                graphics.DrawImage(coinImage.Bitmap,
                    new Rectangle(new Point(row1X, 0), coinImage.Bitmap.Size),
                    new Rectangle(new Point(), coinImage.Bitmap.Size),
                    GraphicsUnit.Pixel);
                DrawText(graphics, "COIN QR CODE", row1X, 0);
                DrawText(graphics, $"Code: {coinImage.Coin}", row1X, 22);
                DrawText(graphics, $"Value: {coinImage.Coin.Value.ToString()}", row1X, 44);
                
                row1X += coinImage.Bitmap.Width;
            }

            var row2X = 0;
            var row2Y = memberQrCodeImageFileInfo.Bitmap.Height + 1;

            // Row 2
            foreach (var coinImage in coinQrCodeImageFileInfos.Skip(2).Take(3))
            {
                graphics.DrawImage(coinImage.Bitmap,
                    new Rectangle(new Point(row2X, row2Y), coinImage.Bitmap.Size),
                    new Rectangle(new Point(), coinImage.Bitmap.Size),
                    GraphicsUnit.Pixel);
                DrawText(graphics, "COIN QR CODE", row2X, row2Y);
                DrawText(graphics, $"Code: {coinImage.Coin}", row2X, 22 + row2Y);
                DrawText(graphics, $"Value: {coinImage.Coin.Value.ToString()}", row2X, 44 + row2Y);
                row2X += coinImage.Bitmap.Width;
            }

            var path = Path.Combine(testSheetsDirectoryInfo.FullName, $"{memberQrCodeImageFileInfo.Member.FullName}_TestSheet.png");
            outputImage.Save(path, ImageFormat.Png);
        }

        memberQrCodeImageFileInfo.Bitmap.Dispose();
        coinQrCodeImageFileInfos.ForEach(x => x.Bitmap.Dispose());
    }

    private void DrawText(Graphics graphics, string text, int x, int y)
    {
        graphics.DrawString(text, new Font(FontFamily.GenericMonospace, 20), new SolidBrush(Color.Black), x, y);
    }
}

public class MemberFileInfo
{
    public Member Member { get; set; }
    public FileInfo FileInfo { get; set; }
    public Bitmap Bitmap { get; set; }
}

public class CoinFileInfo
{
    public Coin Coin { get; set; }
    public FileInfo FileInfo { get; set; }
    public Bitmap Bitmap { get; set; }
}