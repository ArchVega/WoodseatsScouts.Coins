using WoodseatsScouts.Coins.Api.Models.Domain;

namespace WoodseatsScouts.Coins.Api.Models.Dtos.Coins;

public class ScoutSectionDto(ScoutSection scoutSection)
{
    public string Id { get; set; } = scoutSection.Code; // make consistent - Code or Id??
    
    public string Name { get; set; } = scoutSection.Name;
}