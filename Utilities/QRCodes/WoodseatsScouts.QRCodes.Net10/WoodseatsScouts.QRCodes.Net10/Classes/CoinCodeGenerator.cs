using System.Collections.Generic;
using System.Linq;

namespace WoodseatsScouts.QRCodes.Classes;

public class CoinCodeGenerator
{
    public int NumberOfBasesToCreate { get; set; }
    public List<int> FixedCoinValues { get; set; }
    public List<int> RandomCoinValues { get; set; }

    public List<Coin> Generate()
    {
        var id = 1;

        var codes = new List<Coin>();

        for (var basesIndex = 1; basesIndex <= NumberOfBasesToCreate; basesIndex++)
        {
            codes.AddRange(FixedCoinValues.Select(fixedCoinValue => new Coin(id++) { Base = basesIndex, Value = fixedCoinValue }));
            codes.AddRange(RandomCoinValues.Select(randomCoinValue => new Coin(id++) { Base = basesIndex, Value = randomCoinValue }));
        }

        return codes;
    }
}