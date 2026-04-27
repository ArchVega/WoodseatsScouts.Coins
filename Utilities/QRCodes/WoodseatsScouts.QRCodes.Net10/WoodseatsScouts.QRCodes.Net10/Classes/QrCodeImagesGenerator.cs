using QRCoder.Core;
using WoodseatsScouts.QRCodes.Classes;

namespace WoodseatsScouts.QRCodes.Net10.Classes;

public class QrCodeImagesGenerator(FileSystemInfo memberQrCodesDirectoryInfo, FileSystemInfo coinCodesDirectoryInfo) : IDisposable
{
    private readonly QRCodeGenerator qrCodeGenerator = new();
    
    private const int PixelsPerModule = 20;

    private const QRCodeGenerator.ECCLevel EccLevel = QRCodeGenerator.ECCLevel.Q;

    public void GenerateMemberQrCodes(List<ScoutMember> members)
    {
        foreach (var member in members)
        {
            var fullName = Path.Combine(memberQrCodesDirectoryInfo.FullName, $"{member.FullName}.png");
            
            using var qrCodeData = qrCodeGenerator.CreateQrCode(member.Code, EccLevel);
            using var qrCode = new PngByteQRCode(qrCodeData);
            var qrCodeImageBitmap = qrCode.GetGraphic(PixelsPerModule);
            
            File.WriteAllBytes(fullName, qrCodeImageBitmap);
        }
    }

    public void GenerateCoinQrCodes(List<Coin> coins)
    {
        foreach (var coin in coins)
        {
            var fullName = Path.Combine(coinCodesDirectoryInfo.FullName, $"{coin}.png");

            using var qrCodeData = qrCodeGenerator.CreateQrCode(coin.ToString(), EccLevel);
            using var qrCode = new PngByteQRCode(qrCodeData);
            var qrCodeImageBitmap = qrCode.GetGraphic(PixelsPerModule);

            File.WriteAllBytes(fullName, qrCodeImageBitmap);
        }
    }

    public void Dispose()
    {
        qrCodeGenerator.Dispose();
    }
}