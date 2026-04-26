using System.ComponentModel.DataAnnotations;

namespace WoodseatsScouts.Coins.Api.Models.Domain;

public class ActivityBase
{
    public int Id { get; set; }
    
    public required string Name { get; set; }
}