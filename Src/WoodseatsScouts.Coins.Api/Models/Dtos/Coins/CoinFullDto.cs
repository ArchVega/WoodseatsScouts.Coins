namespace WoodseatsScouts.Coins.Api.Models.Dtos.Coins;

public class CoinFullDto
{
    public required int Id { get; set; }
    
    public required int ActivityBaseSequenceNumber { get; set; }
    
    public required int ActivityBaseId { get; set; }
    
    public required int Value { get; set; }

    public required string Code { get; set; }

    public required int? ScoutMemberId { get; set; }
    
    public required DateTime? LockUntil { get; set; }
}