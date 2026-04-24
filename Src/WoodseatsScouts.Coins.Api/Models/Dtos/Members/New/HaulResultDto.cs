namespace WoodseatsScouts.Coins.Api.Models.Dtos.Members.New;

public class HaulResultDto
{
    public int ScavengerResultId { get; set; }
    public string HauledAtIso8601 { get; set; }
    public int TotalPoints { get; set; }

    public List<ActivityBaseHaulResultDto> ActivityBaseHaulResultDtos { get; set; }
}