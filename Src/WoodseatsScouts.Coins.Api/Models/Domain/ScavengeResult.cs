namespace WoodseatsScouts.Coins.Api.Models.Domain;

public class ScavengeResult
{
    public int Id { get; set; }
    
    public int MemberId { get; set; }
    
    public Member Member { get; set; }
    
    public DateTime CompletedAt { get; set; }

    public List<ScavengedCoin> ScavengedCoins { get; set; }
}