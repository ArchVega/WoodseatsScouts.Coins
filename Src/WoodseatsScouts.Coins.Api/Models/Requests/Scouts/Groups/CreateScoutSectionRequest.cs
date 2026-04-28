namespace WoodseatsScouts.Coins.Api.Models.Requests.Scouts.Groups;

public class CreateScoutSectionRequest
{
    public required string Code { get; set; }
    
    public required string Name { get; set; }
}