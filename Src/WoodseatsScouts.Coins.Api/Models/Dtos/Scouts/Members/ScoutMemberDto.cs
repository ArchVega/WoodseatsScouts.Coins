using WoodseatsScouts.Coins.Api.Models.Domain;

namespace WoodseatsScouts.Coins.Api.Models.Dtos.Scouts.Members;

public class ScoutMemberDto(ScoutMember scoutMember) : ScoutMemberBaseDto(scoutMember)
{
    public int ScoutMemberNumber { get; set; } = scoutMember.Number;
    public string FirstName { get; set; } = scoutMember.FirstName;
    public string? LastName { get; set; } = scoutMember.LastName;
    public string FullName { get; set; } = scoutMember.FullName;
    public int ScoutGroupId { get; init; } = scoutMember.ScoutGroupId;
    public string ScoutGroupName { get; init; } = scoutMember.ScoutGroup.Name;
    public string ScoutSectionCode { get; init; } = scoutMember.ScoutSectionCode;
    public string ScoutSectionName { get; set; } = scoutMember.ScoutSection.Name;
    public string? Clue1State { get; init; } = scoutMember.Clue1State;
    public string? Clue2State { get; init; } = scoutMember.Clue2State;
    public string? Clue3State { get; init; } = scoutMember.Clue3State;
    public bool IsDayVisitor { get; set; } = scoutMember.IsDayVisitor;
    public bool HasImage { get; set; } = scoutMember.HasImage;
}