namespace WoodseatsScouts.Coins.Api.Models.Requests.Scouts.Members;

public class UpdateMemberRequest
{
    public required string FirstName { get; set; }
    
    public required string LastName { get; set; }
}