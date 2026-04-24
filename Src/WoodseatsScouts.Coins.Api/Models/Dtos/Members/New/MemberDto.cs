using WoodseatsScouts.Coins.Api.Models.Domain;

namespace WoodseatsScouts.Coins.Api.Models.Dtos.Members.New;

public class MemberDto
{
    public MemberDto(ScoutMember scoutMember)
    {
        var cacheBuster = DateTime.UtcNow.Ticks;

        Id = scoutMember.Id;
        Code = scoutMember.Code;
        Number = scoutMember.Number;
        FirstName = scoutMember.FirstName;
        LastName = scoutMember.LastName;
        FullName = scoutMember.FullName;
        ScoutGroupId = scoutMember.ScoutGroupId;
        ScoutGroupName = scoutMember.ScoutGroup.Name;
        SectionId = scoutMember.ScoutSectionId;
        SectionName = scoutMember.ScoutSection.Name;
        Clue1State = scoutMember.Clue1State;
        Clue2State = scoutMember.Clue2State;
        Clue3State = scoutMember.Clue3State;
        IsDayVisitor = scoutMember.IsDayVisitor;
        HasImage = scoutMember.HasImage;
        ComputedImagePath = scoutMember.HasImage ? $"Members/{scoutMember.Id}/Photo?{cacheBuster}" : "Members/Photo/Placeholder";
    }

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
    public string ComputedImagePath { get; set; }
}