using Microsoft.Data.SqlClient;
using WoodseatsScouts.QRCodes.Classes;

namespace WoodseatsScouts.QRCodes.Net10.Classes;

public class Database : IDisposable
{
    private readonly SqlConnection connection;

    public Database(string databaseName)
    {
        var builder = new SqlConnectionStringBuilder
        {
            ConnectionString = $"Server=localhost,1433;Database={databaseName};User Id=SA;Password=Pa55w0rd123;TrustServerCertificate=True;Encrypt=False"
        };

        this.connection = new SqlConnection(builder.ConnectionString);
        Console.WriteLine("=========================================\n");

        connection.Open();

    }

    public List<ScoutMember> GetMembers()
    {
        const string sql = "SELECT * FROM ScoutMembers";

        using var command = new SqlCommand(sql, connection);
        using var reader = command.ExecuteReader();
        var members = new List<ScoutMember>();
        
        while (reader.Read())
        {
            var member = new ScoutMember
            {
                Code = reader.GetString(1),
                FullName = $"{reader.GetString(3)}{reader.GetString(4)}"
            };
            members.Add(member);
        }

        return members;
    }

    public List<Coin> GetCoinsFromDb(string databaseName)
    {
        var connectionString =
            $"Server=.;Database={databaseName};Trusted_Connection=true;TrustServerCertificate=True";

        const string queryString = "SELECT * from dbo.Coins";

        var coins = new List<Coin>();

        using var command = new SqlCommand(queryString, connection);
        var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var coin = new Coin(Convert.ToInt32(reader[0]))
            {
                ActivityBaseSequenceNumber = Convert.ToInt32(reader[1]),
                ActivityBaseId = Convert.ToInt32(reader[2]),
                Value = Convert.ToInt32(reader[3]),
                Code = reader[4].ToString()
            };

            coins.Add(coin);
        }

        reader.Close();

        return coins;
    }
    
    public void Dispose()
    {
        connection.Dispose();
    }
}