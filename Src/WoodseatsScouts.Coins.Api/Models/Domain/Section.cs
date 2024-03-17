using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WoodseatsScouts.Coins.Api.Models.Domain;

// dotcover disable
public class Section
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Column(TypeName="char(1)")]
    public string Code { get; set; }

    public string Name { get; set; }

    public Section()
    {
    }
    
    public Section(string code, string name)
    {
        Code = code;
        Name = name;
    }
}