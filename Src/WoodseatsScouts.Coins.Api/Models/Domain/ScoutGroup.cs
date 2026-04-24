using System.ComponentModel.DataAnnotations;

namespace WoodseatsScouts.Coins.Api.Models.Domain;

public class ScoutGroup
{
    public int Id { get; set; }
    
    [MaxLength(100)]
    public required string Name { get; set; }

    public required List<ScoutMember> ScoutMembers { get; set; } = [];
}