using System.ComponentModel.DataAnnotations;

namespace WoodseatsScouts.Coins.Api.Models.Domain;

public class ActivityBase
{
    public int Id { get; set; }
    
    [MaxLength(100)]
    public required string Name { get; set; }
}