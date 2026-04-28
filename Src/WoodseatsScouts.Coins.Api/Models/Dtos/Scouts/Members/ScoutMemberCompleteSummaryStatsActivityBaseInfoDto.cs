namespace WoodseatsScouts.Coins.Api.Models.Dtos.Scouts.Members;

public class ScoutMemberCompleteSummaryStatsActivityBaseInfoDto
{
    public string Name { get; set; }

    public int TimesVisited { get; set; }

    public override string ToString()
    {
        return $"{nameof(Name)}: {Name}, {nameof(TimesVisited)}: {TimesVisited}";
    }
}