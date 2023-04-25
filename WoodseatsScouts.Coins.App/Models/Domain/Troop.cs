using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WoodseatsScouts.Coins.App.Models.Domain;

public class Troop
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public List<Member> Members { get; set; }
}