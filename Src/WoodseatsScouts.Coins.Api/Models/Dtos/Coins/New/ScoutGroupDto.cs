using WoodseatsScouts.Coins.Api.Models.Domain;

namespace WoodseatsScouts.Coins.Api.Models.Dtos.Coins.New;

public class ScoutGroupDto(ScoutGroup scoutGroup)
{
    public int Id { get; set; } = scoutGroup.Id;
    
    public string Name { get; set; } = scoutGroup.Name;
}