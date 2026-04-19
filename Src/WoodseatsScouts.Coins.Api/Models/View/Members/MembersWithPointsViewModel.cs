// dotcover disable 

using WoodseatsScouts.Coins.Api.Models.Domain;

namespace WoodseatsScouts.Coins.Api.Models.View.Members;

public class HaulResult
{
    public int ScavengerResultId { get; set; }
    public string HauledAtIso8601 { get; set; }
    public int TotalPoints { get; set; }
}

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
        ScoutGroupName = member.ScoutGroup.Name;
        SectionId = member.SectionId;
        SectionName = member.Section.Name;
        TotalPoints = member.ScavengeResults.SelectMany(y => y.ScavengedCoins.Select(z => z.PointValue)).Sum();
        HaulResults = member.ScavengeResults.Select(x =>
        {
            
            return new HaulResult
            {
                ScavengerResultId = x.Id,
                HauledAtIso8601 = x.CompletedAt.ToUniversalTime().ToString("o"), // ISO 8601
                TotalPoints = x.ScavengedCoins.Sum(x => x.PointValue)
            };
        }).ToList();
    }

    public int? SelectedHaulResultId { get; set; }
    
    public List<HaulResult> HaulResults { get; set; }

    public HaulResult? LatestHaulResult
    {
        get
        {
            if (HaulResults.Count > 0)
            {
                return HaulResults.OrderByDescending(x => x.HauledAtIso8601).First();
            }

            return null;
        }
    }
    
    public HaulResult? SelectedHaulResult
    {
        get
        {
            if (SelectedHaulResultId.HasValue)
            {
                return HaulResults.Single(x => x.ScavengerResultId == SelectedHaulResultId);
            }

            return null;
        }
    }

    public int Id { get; set; }

    public string FullName { get; set; }

    public string MemberCode { get; set; }

    public bool HasImage { get; set; }

    public int MemberNumber { get; set; }

    public string FirstName { get; set; }

    public string? LastName { get; set; }

    public string ScoutGroupName { get; set; }

    public string SectionId { get; set; } // Todo Section is now "SectionId". Rename
    public string SectionName { get; set; }

    public int TotalPoints { get; set; }

    public DateTime? LatestCompletedAtTime { get; set; }
}