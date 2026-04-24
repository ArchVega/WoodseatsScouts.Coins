using WoodseatsScouts.Coins.Api.Models.Domain;

namespace WoodseatsScouts.Coins.Api.Models.Dtos.Coins.New;

public class CoinDto(int pointValue, int activityBaseId, string code)
{
    public string Code { get; set; } = code;

    public int ActivityBaseId { get; set; } = activityBaseId;

    public int PointValue { get; set; } = pointValue;
}

public class ScoutGroupDto(ScoutGroup scoutGroup)
{
    public int Id { get; set; } = scoutGroup.Id;
    
    public string Name { get; set; } = scoutGroup.Name;
}

public class ScoutSectionDto(ScoutSection scoutSection)
{
    public string Id { get; set; } = scoutSection.Code; // make consistent - Code or Id??
    
    public string Name { get; set; } = scoutSection.Name;
}

public class ActivityBasesDto(ActivityBase activityBase)
{
    public int Id { get; set; } = activityBase.Id;
    
    public string Name { get; set; } = activityBase.Name;
}
