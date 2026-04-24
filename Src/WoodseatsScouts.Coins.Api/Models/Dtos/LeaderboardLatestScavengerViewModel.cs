// dotcover disable

using WoodseatsScouts.Coins.Api.Models.Domain;

namespace WoodseatsScouts.Coins.Api.Models.View;

public class LeaderboardLatestScavengerViewModel
{
    public LeaderboardLatestScavengerViewModel(ScoutMember domain)

    {
        Id = domain.Id;
        MemberCode = domain.Code;
        HasImage = domain.HasImage;
        MemberNumber = domain.Number;
        FirstName = domain.FirstName;
        LastName = domain.LastName;
        ScoutGroupName = domain.ScoutGroup.Name;
        Section = domain.ScoutSectionId;
        SectionName = domain.ScoutSection.Name;
        // changed
        // TotalPoints = domain.ScavengeResults.Last().ScavengedCoins.Sum(y => y.PointValue);
        TotalPoints = domain.ScavengeResults.Last().ScanCoins.Sum(y => y.Coin.Value);
    }

    public int TotalPoints { get; set; }

    public string SectionName { get; set; }

    public string Section { get; set; }

    public string ScoutGroupName { get; set; }

    public string? LastName { get; set; }

    public string FirstName { get; set; }

    public int MemberNumber { get; set; }

    public bool HasImage { get; set; }

    public string MemberCode { get; set; }

    public int Id { get; set; }
}