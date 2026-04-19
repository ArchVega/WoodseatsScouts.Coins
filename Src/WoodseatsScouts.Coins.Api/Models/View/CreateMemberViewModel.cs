namespace WoodseatsScouts.Coins.Api.Models.View;

public class CreateMemberViewModel
{
    public string FirstName{ get; set; } 
    
    public string LastName{ get; set; }

    public int ScoutGroupId{ get; set; } 
    
    public string Section{ get; set; }

    public bool IsDayVisitor { get; set; }
}