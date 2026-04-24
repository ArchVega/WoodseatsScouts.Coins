using WoodseatsScouts.Coins.Api.Models.Domain;

namespace WoodseatsScouts.Coins.Api.Models.Dtos.Members;

public class ScoutMemberDto(ScoutMember scoutMember) : MemberBaseDto(scoutMember)
{
    public int Number { get; set; } = scoutMember.Number;
    public string FirstName { get; set; } = scoutMember.FirstName;
    public string? LastName { get; set; } = scoutMember.LastName;
    public string FullName { get; set; } = scoutMember.FullName;
    public int ScoutGroupId { get; init; } = scoutMember.ScoutGroupId;
    public string ScoutGroupName { get; init; } = scoutMember.ScoutGroup.Name;
    public string SectionCode { get; init; } = scoutMember.ScoutSectionId;
    public string SectionName { get; set; } = scoutMember.ScoutSection.Name;
    public string? Clue1State { get; init; } = scoutMember.Clue1State;
    public string? Clue2State { get; init; } = scoutMember.Clue2State;
    public string? Clue3State { get; init; } = scoutMember.Clue3State;
    public bool IsDayVisitor { get; set; } = scoutMember.IsDayVisitor;
    public bool HasImage { get; set; } = scoutMember.HasImage;
}