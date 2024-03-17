using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace WoodseatsScouts.QRCodes.Classes;

public class Database : IDisposable
{
    private readonly SqlConnection connection;

    public Database(string databaseName)
    {
        var builder = new SqlConnectionStringBuilder
        {
            ConnectionString = $"Server=(local);Database={databaseName};Trusted_Connection=true;TrustServerCertificate=true"
        };

        this.connection = new SqlConnection(builder.ConnectionString);
        Console.WriteLine("=========================================\n");

        connection.Open();

    }

    public List<Member> GetMembers()
    {
        const string sql = "SELECT * FROM Members";

        using var command = new SqlCommand(sql, connection);
        using var reader = command.ExecuteReader();
        var members = new List<Member>();
        
        while (reader.Read())
        {
            var member = new Member
            {
                Code = reader.GetString(1),
                FullName = $"{reader.GetString(3)}{reader.GetString(4)}"
            };
            members.Add(member);
        }

        return members;
    }

    public void InsertCoinCodes(List<Coin> coinCodes)
    {
        foreach (var coinCode in coinCodes)
        {
            const string sql = "INSERT INTO Coins (Id, Base, [Value], Code) values (@id, @base, @value, @code)";
            var cmd = new SqlCommand(sql, connection);
            cmd.Parameters.Add("@id", SqlDbType.Int);
            cmd.Parameters.Add("@base", SqlDbType.Int);
            cmd.Parameters.Add("@value", SqlDbType.Int);
            cmd.Parameters.Add("@code", SqlDbType.NVarChar);

            cmd.Parameters["@id"].Value = coinCode.Id;
            cmd.Parameters["@base"].Value = coinCode.Base;
            cmd.Parameters["@value"].Value = coinCode.Value;
            cmd.Parameters["@code"].Value = coinCode.ToString();
            
            cmd.ExecuteNonQuery();    
        }
    }
    
    public void Dispose()
    {
        connection.Dispose();
    }

    public void DeleteExistingCoins()
    {
        const string sql = "DELETE FROM Coins";

        using var command = new SqlCommand(sql, connection);
        command.ExecuteNonQuery();
    }
}