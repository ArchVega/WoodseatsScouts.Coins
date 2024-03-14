namespace WoodseatsScouts.Coins.Api.Models.Domain;

public class Troop
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public List<Member> Members { get; set; }
}