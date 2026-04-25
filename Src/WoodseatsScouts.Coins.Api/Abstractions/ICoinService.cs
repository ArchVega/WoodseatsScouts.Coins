using WoodseatsScouts.Coins.Api.Models.Dtos.Coins;

namespace WoodseatsScouts.Coins.Api.Services;

public interface ICoinService
{
    Task<List<CoinFullDto>> GetCoinFullDtos();
}