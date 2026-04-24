namespace WoodseatsScouts.Coins.Api.Models.View;

public class CreateCoinViewModel
{
    public int Points { get; set; }
    
    public int? BaseId { get; set; }
    
    public string? BaseName { get; set; }
    
    public int Count { get; set; }
}