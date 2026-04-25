namespace WoodseatsScouts.Coins.Api.Models.Requests.Scouts.Members;

public class UpdateScoutMemberRequest
{
    public required string FirstName { get; set; }
    
    public required string LastName { get; set; }
    
    public required int ScoutGroupId { get; set; }
    
    public required string ScoutSectionCode { get; set; }
    
}