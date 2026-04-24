namespace WoodseatsScouts.Coins.Api.Models.Domain;

public class ScoutGroup
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public List<ScoutMember> ScoutMembers { get; set; }
}