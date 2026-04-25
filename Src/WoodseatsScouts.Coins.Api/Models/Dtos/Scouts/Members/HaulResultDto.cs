using WoodseatsScouts.Coins.Api.Models.Dtos.Scouts.Members;

namespace WoodseatsScouts.Coins.Api.Models.Dtos.Members;

public class HaulResultDto
{
    public int ScavengerResultId { get; set; }
    public required string HauledAtIso8601 { get; set; }
    public int TotalPoints { get; set; }

    public List<ActivityBaseHaulResultDto> ActivityBaseHaulResultDtos { get; set; } = [];
}