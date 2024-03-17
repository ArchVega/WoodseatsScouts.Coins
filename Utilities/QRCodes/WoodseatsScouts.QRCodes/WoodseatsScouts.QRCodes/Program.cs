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
            var coinCodeGenerator = new CoinCodeGenerator
            {
                NumberOfBasesToCreate = 100,
                FixedCoinValues = [10, 20],
                RandomCoinValues = [3, 9, 11]
            };

            /* --------------------------------------------------------------- */

            GenerateDirectoriesIfRequired();

            var database = new Database(databaseName);
            var qrCodeImagesGenerator = new QrCodeImagesGenerator(generatedMemberQrCodesDirectoryInfo, generatedCoinQrCodesDirectoryInfo);
            var testSheetGenerator = new TestSheetGenerator(databaseDirectoryInfo, testSheetsDirectoryInfo);
            
            var members = database.GetMembers();
            qrCodeImagesGenerator.GenerateMemberQrCodes(members);

            database.DeleteExistingCoins();
            var coins = coinCodeGenerator.Generate();
            WriteCoinCodesToCsv(coins);
            database.InsertCoinCodes(coins);
            qrCodeImagesGenerator.GenerateCoinQrCodes(coins);

            testSheetGenerator.Generate(members, coins);
            
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