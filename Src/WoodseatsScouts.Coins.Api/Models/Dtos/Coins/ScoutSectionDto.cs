using WoodseatsScouts.Coins.Api.Models.Domain;

namespace WoodseatsScouts.Coins.Api.Models.Dtos.Coins;

public class ScoutSectionDto(ScoutSection scoutSection)
{
    public string Code { get; set; } = scoutSection.Code;
    
    public string Name { get; set; } = scoutSection.Name;
}