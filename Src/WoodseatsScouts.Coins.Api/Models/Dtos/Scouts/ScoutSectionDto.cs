using WoodseatsScouts.Coins.Api.Models.Domain;

namespace WoodseatsScouts.Coins.Api.Models.Dtos.Scouts;

public class ScoutSectionDto
{
    public ScoutSectionDto()
    {
    }
    
    public ScoutSectionDto(ScoutSection scoutSection)
    {
        Code = scoutSection.Code;
        Name = scoutSection.Name;
    }

    public string Code { get; set; }
    
    public string Name { get; set; }
}