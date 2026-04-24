using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WoodseatsScouts.Coins.Api.Models.Domain;

// dotcover disable
public class ScoutSection
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Column(TypeName="char(1)")]
    public required string Code { get; set; }

    public required string Name { get; set; }
}