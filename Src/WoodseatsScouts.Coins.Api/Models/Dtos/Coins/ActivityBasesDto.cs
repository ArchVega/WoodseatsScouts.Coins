using WoodseatsScouts.Coins.Api.Models.Domain;

namespace WoodseatsScouts.Coins.Api.Models.Dtos.Coins;

public class ActivityBasesDto
{
    public ActivityBasesDto()
    {
    }
    
    public ActivityBasesDto(ActivityBase activityBase)
    {
        Id = activityBase.Id;
        Name = activityBase.Name;
    }

    public int Id { get; set; }
    
    public string Name { get; set; }
}