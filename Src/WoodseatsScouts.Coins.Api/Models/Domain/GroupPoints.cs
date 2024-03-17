namespace WoodseatsScouts.Coins.Api.Models.Domain;

// dotcover disable
public class GroupPoints
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int TotalPoints { get; set; }
    public int MemberCount { get; set; }

    public double AveragePoints => TotalPoints / (float)MemberCount;

    public override string ToString()
    {
        return $"{nameof(Name)}: {Name}, {nameof(TotalPoints)}: {TotalPoints}, {nameof(MemberCount)}: {MemberCount}, {nameof(AveragePoints)}: {AveragePoints}";
    }
}