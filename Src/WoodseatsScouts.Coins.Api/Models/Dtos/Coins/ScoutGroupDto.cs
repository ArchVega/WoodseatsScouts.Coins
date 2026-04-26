using WoodseatsScouts.Coins.Api.Models.Domain;

namespace WoodseatsScouts.Coins.Api.Models.Dtos.Coins;

public class ScoutGroupDto(ScoutGroup scoutGroup)
{
    public int Id { get; set; } = scoutGroup.Id;
    
    public string Name { get; set; } = scoutGroup.Name;
}