// dotcover disable
using WoodseatsScouts.Coins.Api.Models.Domain;

namespace WoodseatsScouts.Coins.Api.Models.View;

// Todo: convert rest to primary constructor
public class MemberViewModel(Member member)
{
    public string FirstName { get; set; } = member.FirstName;

    public string? LastName { get; set; } = member.LastName;

    public string MemberPhotoPath { get; set; } = $"/member-images/{member.Id}.jpg"; // Todo: hardcoded path

    public int MemberTroopNumber { get; set; } = member.TroopId;

    public string MemberSection { get; set; } = member.SectionId;

    public int MemberId { get; set; } = member.Id;

    public string MemberCode { get; set; } = member.Code;

    public int MemberNumber { get; set; } = member.Number;
}