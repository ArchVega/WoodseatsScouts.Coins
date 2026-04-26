using System.ComponentModel.DataAnnotations;

namespace WoodseatsScouts.Coins.Api.Models.Requests.Scouts.Members;

public class CreateMemberRequest
{
    [Required]
    public required string FirstName{ get; set; } 
    
    public string LastName{ get; set; } = null!;

    public int ScoutGroupId{ get; set; } 
    
    public required string SectionCode{ get; set; }

    public bool IsDayVisitor { get; set; }
}