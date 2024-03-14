namespace WoodseatsScouts.Coins.Api.Models.Domain;

// dotcover disable
public class GroupPoints
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int TotalPoints { get; set; }
    public int MemberCount { get; set; }

    public double AveragePoints => TotalPoints / (float)MemberCount;
}