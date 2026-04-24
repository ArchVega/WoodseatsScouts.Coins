using WoodseatsScouts.Coins.Api.Models.Domain;

namespace WoodseatsScouts.Coins.Api.Models.Dtos.Members;

public abstract class MemberBaseDto
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string ComputedImagePath { get; set; }

    protected MemberBaseDto(ScoutMember scoutMember)
    {
        var cacheBuster = DateTime.UtcNow.Ticks;
        
        Id = scoutMember.Id;
        Code = scoutMember.Code;
        ComputedImagePath = scoutMember.HasImage ? $"scouts/members/{scoutMember.Id}/photo?{cacheBuster}" : "scouts/members/photo/placeholder";
    }
}