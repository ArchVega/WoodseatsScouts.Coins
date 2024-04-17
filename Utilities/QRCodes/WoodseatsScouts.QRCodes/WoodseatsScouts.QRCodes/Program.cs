using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WoodseatsScouts.QRCodes.Classes;

namespace WoodseatsScouts.QRCodes
{
    internal class Program
    {
        private DirectoryInfo databaseDirectoryInfo;
        private DirectoryInfo outputRootDirectoryInfo;
        private DirectoryInfo testSheetsDirectoryInfo;
        private DirectoryInfo generatedMemberQrCodesDirectoryInfo;
        private DirectoryInfo generatedCoinQrCodesDirectoryInfo;

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
            /* --------------------------------------------------------------- */

            GenerateDirectoriesIfRequired();

            var database = new Database(databaseName);
            var qrCodeImagesGenerator = new QrCodeImagesGenerator(generatedMemberQrCodesDirectoryInfo, generatedCoinQrCodesDirectoryInfo);
            var testSheetGenerator = new TestSheetGenerator(databaseDirectoryInfo, testSheetsDirectoryInfo);
            
            var members = database.GetMembers();
            qrCodeImagesGenerator.GenerateMemberQrCodes(members);
            
            var coins = database.GetCoinsFromDb(databaseName);
            WriteCoinCodesToCsv(coins);
            qrCodeImagesGenerator.GenerateCoinQrCodes(coins.Take(members.Count * 12).ToList());

            testSheetGenerator.Generate(members, coins.Take(members.Count * 12).ToList());
            
            Console.WriteLine("\nDone");
        }

        private void WriteCoinCodesToCsv(IEnumerable<Coin> coinCodes)
        {
            var fileName = Path.Combine(databaseDirectoryInfo.FullName, "CoinCodes.csv");
            var contents = coinCodes
                .Select(x => $"{x},{x.Id},{x.Base},{x.Value}")
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
            generatedMemberQrCodesDirectoryInfo = Directory.CreateDirectory(Path.Combine(databaseDirectoryInfo.FullName, "QRCodes", "Members"));
            generatedCoinQrCodesDirectoryInfo = Directory.CreateDirectory(Path.Combine(databaseDirectoryInfo.FullName, "QRCodes", "Coins"));
        }
    }
}