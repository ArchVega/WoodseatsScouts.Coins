using WoodseatsScouts.Coins.Api.Models.Domain;

namespace WoodseatsScouts.Coins.Api.Models.Dtos.Scouts.Members;

public class ScoutMemberPointsSummaryDto
{
    public ScoutMemberPointsSummaryDto(ScoutMember scoutMember)
    {
        var cacheBuster = DateTime.UtcNow.Ticks;
        
        Id = scoutMember.Id;
        ScoutMemberCode = scoutMember.Code;
        HasImage = scoutMember.HasImage;
        ComputedImagePath = scoutMember.HasImage ? $"scouts/members/{scoutMember.Id}/photo?{cacheBuster}" : "scouts/members/photo/placeholder";
        MemberNumber = scoutMember.Number;
        FirstName = scoutMember.FirstName;
        LastName = scoutMember.LastName;
        FullName = scoutMember.FullName;
        ScoutGroupId = scoutMember.ScoutGroup.Id;
        ScoutGroupName = scoutMember.ScoutGroup.Name;
        ScoutSectionCode = scoutMember.ScoutSectionCode;
        ScoutSectionName = scoutMember.ScoutSection.Name;
        TotalPoints = scoutMember.ScanSessions.SelectMany(y => y.ScanCoins.Select(z => z.Coin!.Value)).Sum();
        HaulResults = scoutMember.ScanSessions.Select(x =>
        {
            return new HaulResultDto
            {
                ScanSessionId = x.Id,
                HauledAtIso8601 = x.CompletedAt.ToUniversalTime().ToString("o"),
                TotalPoints = x.ScanCoins.Sum(x => x.Coin!.Value)
            };
        }).ToList();
    }

    public int Id { get; set; }

    public string FullName { get; set; }

    public string ScoutMemberCode { get; set; }

    public bool HasImage { get; set; }
    
    public string ComputedImagePath { get; set; }

    public int MemberNumber { get; set; }

    public string FirstName { get; set; }

    public string? LastName { get; set; }

    public int ScoutGroupId { get; init; }
    public string ScoutGroupName { get; set; }

    public string ScoutSectionCode { get; set; }
    
    public string ScoutSectionName { get; set; }

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
                return HaulResults.Single(x => x.ScanSessionId == SelectedHaulResultId);
            }

            return null;
        }
    }
}

