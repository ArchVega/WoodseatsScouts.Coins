using WoodseatsScouts.QRCodes.Classes;
using WoodseatsScouts.QRCodes.Net10.Classes;

namespace WoodseatsScouts.QRCodes.Net10;

/*
--------------------------------------------------------------------------------
Ubuntu 25.10 - REQUIRED: sudo apt install libgdiplus
--------------------------------------------------------------------------------
*/

internal class Program
{
    private DirectoryInfo databaseDirectoryInfo = null!;
    private DirectoryInfo outputRootDirectoryInfo = null!;
    private DirectoryInfo testSheetsDirectoryInfo = null!;
    private DirectoryInfo testSheetsPaddedDirectoryInfo = null!;
    private DirectoryInfo generatedMemberQrCodesDirectoryInfo = null!;
    private DirectoryInfo generatedCoinQrCodesDirectoryInfo = null!;

    private readonly string databaseName;
    private readonly string outputRootDirectory;

    public static void Main(string[] args)
    {
        var databaseName = args[0];
        var outputRootDirectory = args[1];

        var program = new Program(databaseName, outputRootDirectory);
        program.Run();
    }

    private void Run()
    {
        GenerateDirectoriesIfRequired();

        var database = new Database(databaseName);
        var qrCodeImagesGenerator = new QrCodeImagesGenerator(generatedMemberQrCodesDirectoryInfo, generatedCoinQrCodesDirectoryInfo);
        var testSheetGenerator = new TestSheetGenerator(databaseDirectoryInfo, testSheetsDirectoryInfo, testSheetsPaddedDirectoryInfo);
            
        var members = database.GetMembers();
        qrCodeImagesGenerator.GenerateMemberQrCodes(members);
            
        var coins = database.GetCoinsFromDb(databaseName);
        WriteCoinCodesToCsv(coins);
        Console.WriteLine("Generating QR Codes...");
        qrCodeImagesGenerator.GenerateCoinQrCodes(coins.Take(members.Count * 12).ToList());

        Console.WriteLine("Generating test sheet...");
        testSheetGenerator.Generate(members, coins.Take(members.Count * 12).ToList());
            
        Console.WriteLine("\nDone");
    }

    private void WriteCoinCodesToCsv(IEnumerable<Coin> coinCodes)
    {
        var fileName = Path.Combine(databaseDirectoryInfo.FullName, "CoinCodes.csv");
        var contents = coinCodes
            .Select(x => $"{x},{x.Id},{x.ActivityBaseId},{x.Value}")
            .Aggregate((x, y) => $"{x}{Environment.NewLine}{y}");
        contents = $"Code,Id,Base,Value{Environment.NewLine}" + contents;
        File.WriteAllText(fileName, contents);
    }

    private Program(string databaseName, string outputRootDirectory)
    {
        this.databaseName = databaseName;
        this.outputRootDirectory = outputRootDirectory;
    }

    private void GenerateDirectoriesIfRequired()
    {
        this.outputRootDirectoryInfo = new DirectoryInfo(outputRootDirectory);
        if (!outputRootDirectoryInfo.Exists)
        {
            throw new Exception($"Directory {outputRootDirectory} does not exist");
        }

        databaseDirectoryInfo = new DirectoryInfo(Path.Combine(outputRootDirectoryInfo.FullName, databaseName));

        if (databaseDirectoryInfo.Exists)
        {
            databaseDirectoryInfo.Delete(true);
        }

        databaseDirectoryInfo.Create();
        testSheetsDirectoryInfo = Directory.CreateDirectory(Path.Combine(databaseDirectoryInfo.FullName, "TestSheets"));
        testSheetsDirectoryInfo.Create();
        testSheetsPaddedDirectoryInfo = Directory.CreateDirectory(Path.Combine(databaseDirectoryInfo.FullName, "TestSheets-Padded"));
        testSheetsPaddedDirectoryInfo.Create();
        generatedMemberQrCodesDirectoryInfo = Directory.CreateDirectory(Path.Combine(databaseDirectoryInfo.FullName, "QRCodes", "Members"));
        generatedCoinQrCodesDirectoryInfo = Directory.CreateDirectory(Path.Combine(databaseDirectoryInfo.FullName, "QRCodes", "Coins"));
    }
}