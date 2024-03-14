using Microsoft.Data.SqlClient;
using QRCoder;

internal class Program
{
    public static void Main(string[] args)
    {
        try
        {
            var databaseName = args[0];
            var outputRootDirectory = args[1];
            
            Console.WriteLine($"Generating data and QR codes for {databaseName}");
            var builder = new SqlConnectionStringBuilder
            {
                ConnectionString = $"Server=(local);Database={databaseName};Trusted_Connection=true;TrustServerCertificate=true"
            };

            using var connection = new SqlConnection(builder.ConnectionString);
            Console.WriteLine("=========================================\n");

            connection.Open();

            GenerateDirectoriesIfRequired(outputRootDirectory, databaseName);
            GenerateQRCodes(connection);
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.ToString());
        }

        Console.WriteLine("\nDone. Press enter.");
        Console.ReadLine();
    }

    private static void GenerateDirectoriesIfRequired(string outputRootDirectory, string databaseName)
    {
        var outputRootDirectoryInfo = new DirectoryInfo(outputRootDirectory);
        if (!outputRootDirectoryInfo.Exists)
        {
            throw new Exception($"Directory {outputRootDirectory} does not exist");
        }

        var databaseDirectoryInfo = new DirectoryInfo(Path.Join(outputRootDirectoryInfo.FullName, databaseName));

        if (databaseDirectoryInfo.Exists) return;
        
        databaseDirectoryInfo.Create();
        Directory.CreateDirectory(Path.Join(databaseDirectoryInfo.FullName, "QRCodes"));
    }

    private static void GenerateQRCodes(SqlConnection connection)
    {
        const string sql = "SELECT * FROM Members";

        using var command = new SqlCommand(sql, connection);
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            using var qrCodeGenerator = new QRCodeGenerator();
            using var qrCodeData = qrCodeGenerator.CreateQrCode(reader.GetString(1), QRCodeGenerator.ECCLevel.Q);
            using (QRCode qrCode = new QRCode(qrCodeData))
            {
                Bitmap qrCodeImage = qrCode.GetGraphic(20);
            }
        }
    }
}