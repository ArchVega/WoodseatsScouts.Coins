using System.ComponentModel.DataAnnotations;
using WoodseatsScouts.Coins.Api.Models.Domain;

namespace WoodseatsScouts.QRCodes.Classes;

public class Coin(int i)
{
    public int Id { get; set; }
    
    public int ActivityBaseSequenceNumber { get; set; }
    
    public int ActivityBaseId { get; set; }
    
    public ActivityBase? ActivityBase { get; set; }

    public int Value { get; set; }

    public string Code { get; set; } = null!;
    
    public override string ToString()
    {
        return Code;
    }
}