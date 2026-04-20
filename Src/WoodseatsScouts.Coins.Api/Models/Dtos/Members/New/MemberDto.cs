namespace WoodseatsScouts.Coins.Api.Models.Dtos.Members.New;

public class MemberDto
{
    public int Id { get; set; }
    public string Code { get; set; }
    public int Number { get; set; }
    public string FirstName { get; set; }
    public string? LastName { get; set; }
    public string FullName { get; set; }
    public int ScoutGroupId { get; init; }
    public string ScoutGroupName { get; init; }
    public string SectionId { get; init; }
    public string SectionName { get; set; }
    public string? Clue1State { get; init; }
    public string? Clue2State { get; init; }
    public string? Clue3State { get; init; }
    public bool IsDayVisitor { get; set; }
    public bool HasImage { get; set; }
}