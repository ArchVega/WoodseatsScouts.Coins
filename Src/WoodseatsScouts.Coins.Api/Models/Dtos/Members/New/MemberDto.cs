using WoodseatsScouts.Coins.Api.Models.Domain;

namespace WoodseatsScouts.Coins.Api.Models.Dtos.Members.New;

public class MemberDto
{
    public MemberDto(Member member)
    {
        var cacheBuster = DateTime.Now.Ticks;

        Id = member.Id;
        Code = member.Code;
        Number = member.Number;
        FirstName = member.FirstName;
        LastName = member.LastName;
        FullName = member.FullName;
        ScoutGroupId = member.ScoutGroupId;
        ScoutGroupName = member.ScoutGroup.Name;
        SectionId = member.SectionId;
        SectionName = member.Section.Name;
        Clue1State = member.Clue1State;
        Clue2State = member.Clue2State;
        Clue3State = member.Clue3State;
        IsDayVisitor = member.IsDayVisitor;
        HasImage = member.HasImage;
        ImagePath = member.HasImage ? $"Members/{member.Id}/Photo?{cacheBuster}" : "Members/Photo/Placeholder";
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
    public string ImagePath { get; set; }
}