using WoodseatsScouts.Coins.Api.Models.Domain;
using WoodseatsScouts.Coins.Api.Models.View.Members;

namespace WoodseatsScouts.Coins.Api.Models.Dtos.Members.New;

// Src/WoodseatsScouts.Coins.Api/Models/Dtos/Members/MembersWithPointsViewModel.cs
public class MemberPointsSummaryDto
{
    public MemberPointsSummaryDto(ScoutMember scoutMember)
    {
        var cacheBuster = DateTime.UtcNow.Ticks;
        
        Id = scoutMember.Id;
        MemberCode = scoutMember.Code;
        HasImage = scoutMember.HasImage;
        ComputedImagePath = scoutMember.HasImage ? $"{scoutMember.Id}/photo?{cacheBuster}" : "photo/placeholder";
        MemberNumber = scoutMember.Number;
        FirstName = scoutMember.FirstName;
        LastName = scoutMember.LastName;
        FullName = scoutMember.FullName;
        ScoutGroupId = scoutMember.ScoutGroup.Id;
        ScoutGroupName = scoutMember.ScoutGroup.Name;
        SectionId = scoutMember.ScoutSectionId;
        SectionName = scoutMember.ScoutSection.Name;
        TotalPoints = scoutMember.ScavengeResults.SelectMany(y => y.ScanCoins.Select(z => z.Coin.Value)).Sum();
        HaulResults = scoutMember.ScavengeResults.Select(x =>
        {
            return new HaulResultDto
            {
                ScavengerResultId = x.Id,
                HauledAtIso8601 = x.CompletedAt.ToUniversalTime().ToString("o"), // ISO 8601
                TotalPoints = x.ScanCoins.Sum(x => x.Coin.Value) // changed
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

    public int ScoutGroupId { get; init; }
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

