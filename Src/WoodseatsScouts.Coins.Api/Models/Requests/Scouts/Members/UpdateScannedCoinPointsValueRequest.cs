using System.ComponentModel.DataAnnotations;

namespace WoodseatsScouts.Coins.Api.Models.Requests.Scouts.Members;

public class UpdateScannedCoinPointsValueRequest
{
    [Range(1, int.MaxValue)]
    public int NewPointsValue { get; set; }
}