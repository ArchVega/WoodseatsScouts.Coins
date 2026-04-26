using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

namespace WoodseatsScouts.Coins.Api.Models.Domain;

// dotcover disable
public class ScoutSection
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Column(TypeName="char(1)")]
    public string Code { get; set; }

    public required string Name { get; set; }
}