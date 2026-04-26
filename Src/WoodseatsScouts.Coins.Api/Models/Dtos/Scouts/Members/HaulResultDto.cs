namespace WoodseatsScouts.Coins.Api.Models.Dtos.Scouts.Members;

public class HaulResultDto
{
    public int ScanSessionId { get; set; }
    
    public required string HauledAtIso8601 { get; set; }
    
    public int TotalPoints { get; set; }

    public List<ActivityBaseHaulResultDto> ActivityBaseHaulResultDtos { get; set; } = [];
}