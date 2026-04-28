using WoodseatsScouts.Coins.Api.Models.Domain;

namespace WoodseatsScouts.Coins.Api.Models.Dtos.Scouts;

public class ScoutGroupDto
{
    public ScoutGroupDto()
    {
    }
    
    public ScoutGroupDto(ScoutGroup scoutGroup)
    {
        Id = scoutGroup.Id;
        Name = scoutGroup.Name;
    }

    public int Id { get; set; }
    
    public string Name { get; set; }
}