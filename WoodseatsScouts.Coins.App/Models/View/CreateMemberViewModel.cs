namespace WoodseatsScouts.Coins.App.Models;

public class CreateMemberViewModel
{
    public string FirstName{ get; set; } 
    
    public string LastName{ get; set; }

    public int TroopId{ get; set; } 
    
    public string Section{ get; set; }

    public bool IsDayVisitor { get; set; }
}