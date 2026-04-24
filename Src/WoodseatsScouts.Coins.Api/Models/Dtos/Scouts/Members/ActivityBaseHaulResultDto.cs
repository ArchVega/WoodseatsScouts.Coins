using System.Diagnostics.CodeAnalysis;
using WoodseatsScouts.Coins.Api.Models.Dtos.Coins;

namespace WoodseatsScouts.Coins.Api.Models.Dtos.Scouts.Members;

[SuppressMessage("ReSharper", "EntityFramework.ModelValidation.UnlimitedStringLength")]
[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class ActivityBaseHaulResultDto
{
    public int ActivityBaseId { get; set; }
 
    public required string ActivityBaseName { get; set; }
    
    public int TotalPoints { get; set; }
    
    public int CoinsScanned { get; set; }
    
    public required List<CoinDto> Coins { get; set; }
}