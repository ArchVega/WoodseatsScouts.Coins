// dotcover disable 
using WoodseatsScouts.Coins.Api.Models.Domain;

namespace WoodseatsScouts.Coins.Api.Models.View;

public class MembersWithPointsViewModel
{
    public MembersWithPointsViewModel(Member member)
    {
        Id = member.Id;
        MemberCode = member.Code;
        HasImage = member.HasImage;
        MemberNumber = member.Number;
        FirstName = member.FirstName;
        LastName = member.LastName;
        FullName = member.FullName;
        TroopName = member.Troop.Name;
        Section = member.SectionId;
        SectionName = member.Section.Name;
        TotalPoints = member.ScavengeResults.SelectMany(y => y.ScavengedCoins.Select(z => z.PointValue)).Sum();
    }

    public string FullName { get; set; }

    public string MemberCode { get; set; }

    public bool HasImage { get; set; }

    public int MemberNumber { get; set; }

    public string FirstName { get; set; }

    public string? LastName { get; set; }

    public string TroopName { get; set; }

    public string Section { get; set; } // Todo Section is now "SectionId". Rename
    public string SectionName { get; set; }

    public int TotalPoints { get; set; }


    public int Id { get; set; }
}