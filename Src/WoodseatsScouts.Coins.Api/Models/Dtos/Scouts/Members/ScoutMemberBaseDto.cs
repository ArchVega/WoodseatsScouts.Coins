using WoodseatsScouts.Coins.Api.Models.Domain;

namespace WoodseatsScouts.Coins.Api.Models.Dtos.Scouts.Members;

public abstract class ScoutMemberBaseDto
{
    public int Id { get; set; }
    public string ScoutMemberCode { get; set; }
    public string ComputedImagePath { get; set; }

    protected ScoutMemberBaseDto(ScoutMember scoutMember)
    {
        var cacheBuster = DateTime.UtcNow.Ticks;
        
        Id = scoutMember.Id;
        ScoutMemberCode = scoutMember.Code;
        ComputedImagePath = scoutMember.HasImage ? $"scouts/members/{scoutMember.Id}/photo?{cacheBuster}" : "scouts/members/photo/placeholder";
    }
}