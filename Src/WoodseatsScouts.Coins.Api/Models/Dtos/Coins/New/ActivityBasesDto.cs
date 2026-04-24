using WoodseatsScouts.Coins.Api.Models.Domain;

namespace WoodseatsScouts.Coins.Api.Models.Dtos.Coins.New;

public class ActivityBasesDto(ActivityBase activityBase)
{
    public int Id { get; set; } = activityBase.Id;
    
    public string Name { get; set; } = activityBase.Name;
}