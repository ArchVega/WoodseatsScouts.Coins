namespace WoodseatsScouts.Coins.Api.Models.Dtos.Coins.New;

/// <summary>
/// Either ActivityBaseId or ActivityBaseName must be supplied, not both.
/// </summary>
public class CreateCoinDto
{
    public int PointsPerCoin { get; set; }
    
    public int? ActivityBaseId { get; set; }
    
    public string? ActivityBaseName { get; set; }
    
    public int NumberToCreate { get; set; }
}