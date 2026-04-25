using Microsoft.EntityFrameworkCore;
using WoodseatsScouts.Coins.Api.Abstractions;
using WoodseatsScouts.Coins.Api.Data;
using WoodseatsScouts.Coins.Api.Models.Dtos.Coins;

namespace WoodseatsScouts.Coins.Api.Services;

public class CoinService(IAppDbContext appDbContext) : ICoinService
{
    public async Task<List<CoinFullDto>> GetCoinFullDtos()
    {
        return (await appDbContext.Coins.AsNoTracking().ToListAsync())
            .Select(x => new CoinFullDto
            {
                Id = x.Id,
                ActivityBaseSequenceNumber = x.ActivityBaseSequenceNumber,
                ActivityBaseId = x.ActivityBaseId,
                Value = x.Value,
                Code = x.Code,
                ScoutMemberId = x.MemberId,
                LockUntil = x.LockUntil
            })
            .ToList();
    }
}