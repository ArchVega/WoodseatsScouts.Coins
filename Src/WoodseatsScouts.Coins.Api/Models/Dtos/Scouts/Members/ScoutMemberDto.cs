using WoodseatsScouts.Coins.Api.Models.Domain;

namespace WoodseatsScouts.Coins.Api.Models.Dtos.Scouts.Members;

public class ScoutMemberDto : ScoutMemberBaseDto
{
    public ScoutMemberDto()
    {
    }
    
    public ScoutMemberDto(ScoutMember scoutMember) : base(scoutMember)
    {
        ScoutMemberNumber = scoutMember.Number;
        FirstName = scoutMember.FirstName;
        LastName = scoutMember.LastName;
        FullName = scoutMember.FullName;
        ScoutGroupId = scoutMember.ScoutGroupId;
        ScoutGroupName = scoutMember.ScoutGroup.Name;
        ScoutSectionCode = scoutMember.ScoutSectionCode;
        ScoutSectionName = scoutMember.ScoutSection.Name;
        Clue1State = scoutMember.Clue1State;
        Clue2State = scoutMember.Clue2State;
        Clue3State = scoutMember.Clue3State;
        IsDayVisitor = scoutMember.IsDayVisitor;
        HasImage = scoutMember.HasImage;
    }

    public int ScoutMemberNumber { get; set; }
    public string FirstName { get; set; }
    public string? LastName { get; set; }
    public string FullName { get; set; }
    public int ScoutGroupId { get; init; }
    public string ScoutGroupName { get; init; }
    public string ScoutSectionCode { get; init; }
    public string ScoutSectionName { get; set; }
    public string? Clue1State { get; init; }
    public string? Clue2State { get; init; }
    public string? Clue3State { get; init; }
    public bool IsDayVisitor { get; set; }
    public bool HasImage { get; set; }
}