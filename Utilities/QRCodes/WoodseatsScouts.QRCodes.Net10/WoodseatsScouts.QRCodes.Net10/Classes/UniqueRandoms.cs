using System;
using System.Collections.Generic;

namespace WoodseatsScouts.QRCodes.Classes;

public class UniqueRandoms(Random random)
{
    public List<int> Get(int listMax, int sourceListLength)
    {
        var list = new List<int>();

        for (var i = 0; i < listMax; i++)
        {
            var shouldContinue = true;
            while (shouldContinue)
            {
                var randomInteger = random.Next(sourceListLength);

                if (list.Contains(randomInteger)) continue;
                
                list.Add(randomInteger);
                shouldContinue = false;
            }
        }

        return list;
    }
}