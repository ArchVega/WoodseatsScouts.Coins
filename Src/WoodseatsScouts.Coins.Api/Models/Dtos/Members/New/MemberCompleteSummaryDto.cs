using WoodseatsScouts.Coins.Api.Models.Domain;
using WoodseatsScouts.Coins.Api.Models.View.Members;

namespace WoodseatsScouts.Coins.Api.Models.Dtos.Members.New;

public class MemberCompleteSummaryDto
{
    public MemberCompleteSummaryDto(Member member)
    {
        var cacheBuster = DateTime.UtcNow.Ticks;
        
        Id = member.Id;
        MemberCode = member.Code;
        HasImage = member.HasImage;
        ComputedImagePath = member.HasImage ? $"Members/{member.Id}/Photo?{cacheBuster}" : "Members/Photo/Placeholder";
        MemberNumber = member.Number;
        FirstName = member.FirstName;
        LastName = member.LastName;
        FullName = member.FullName;
        ScoutGroupName = member.ScoutGroup.Name;
        SectionId = member.SectionId;
        SectionName = member.Section.Name;
        // changed
        TotalPoints = member.ScavengeResults.SelectMany(y => y.ScavengedCoins.Select(z => z.Coin.Value)).Sum();
        HaulResults = member.ScavengeResults.Select(x =>
        {
            return new HaulResultDto
            {
                ScavengerResultId = x.Id,
                HauledAtIso8601 = x.CompletedAt.ToUniversalTime().ToString("o"), // ISO 8601
                TotalPoints = x.ScavengedCoins.Sum(x => x.Coin.Value) // changed
            };
        }).ToList();
    }

    public int Id { get; set; }

    public string FullName { get; set; }

    public string MemberCode { get; set; }

    public bool HasImage { get; set; }
    
    public string ComputedImagePath { get; set; }

    public int MemberNumber { get; set; }

    public string FirstName { get; set; }

    public string? LastName { get; set; }

    public string ScoutGroupName { get; set; }

    public string SectionId { get; set; } // Todo Section is now "SectionId". Rename
    public string SectionName { get; set; }

    public int TotalPoints { get; set; }

    public DateTime? LatestCompletedAtTime { get; set; }
    
    public int? SelectedHaulResultId { get; set; }
    
    public List<HaulResultDto> HaulResults { get; set; }

    public HaulResultDto? LatestHaulResult
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
    
    public HaulResultDto? SelectedHaulResult
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
}