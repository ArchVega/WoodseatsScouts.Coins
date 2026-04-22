using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WoodseatsScouts.Coins.Api.Models.Domain;

// dotcover disable
public class ScoutSection
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Column(TypeName="char(1)")]
    public string Code { get; set; }

    public string Name { get; set; }

    public ScoutSection()
    {
    }
    
    public ScoutSection(string code, string name)
    {
        Code = code;
        Name = name;
    }
}