using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using QRCoder;

namespace WoodseatsScouts.QRCodes.Classes;

public class QrCodeImagesGenerator(FileSystemInfo memberQrCodesDirectoryInfo, FileSystemInfo coinCodesDirectoryInfo) : IDisposable
{
    private readonly QRCodeGenerator qrCodeGenerator = new();
    
    private const int PixelsPerModule = 20;

    private const QRCodeGenerator.ECCLevel EccLevel = QRCodeGenerator.ECCLevel.Q;

    public void GenerateMemberQrCodes(List<Member> members)
    {
        foreach (var member in members)
        {
            using var qrCodeData = qrCodeGenerator.CreateQrCode(member.Code, EccLevel);
            using var qrCode = new QRCode(qrCodeData);
            var qrCodeImageBitmap = qrCode.GetGraphic(PixelsPerModule);
            var fullName = Path.Combine(memberQrCodesDirectoryInfo.FullName, $"{member.FullName}.png");
            qrCodeImageBitmap.Save(fullName, ImageFormat.Png);
        }
    }

    public void GenerateCoinQrCodes(List<Coin> coins)
    {
        foreach (var coin in coins)
        {
            using var qrCodeData = qrCodeGenerator.CreateQrCode(coin.ToString(), EccLevel);
            using var qrCode = new QRCode(qrCodeData);
            var qrCodeImageBitmap = qrCode.GetGraphic(PixelsPerModule);
            var fullName = Path.Combine(coinCodesDirectoryInfo.FullName, $"{coin}.png");
            qrCodeImageBitmap.Save(fullName, ImageFormat.Png);
        }
    }

    public void Dispose()
    {
        qrCodeGenerator.Dispose();
    }
}