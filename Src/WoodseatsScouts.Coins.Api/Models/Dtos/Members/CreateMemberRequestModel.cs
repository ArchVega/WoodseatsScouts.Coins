namespace WoodseatsScouts.Coins.Api.Models.Dtos.Members;

public class CreateMemberRequestModel
{
    public string FirstName{ get; set; } 
    
    public string LastName{ get; set; }

    public int ScoutGroupId{ get; set; } 
    
    public string Section{ get; set; }

    public bool IsDayVisitor { get; set; }
}