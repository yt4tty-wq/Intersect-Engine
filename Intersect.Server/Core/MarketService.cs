using System;
using System.Collections.Generic;

namespace Intersect.Server.Core;

public static class MarketService
{
    public static List<MarketItem> Listings { get; } = new();

    public static void AddTestItem()
    {
        Listings.Add(new MarketItem()
        {
            SellerId = Guid.Empty,
            ItemId = Guid.Empty,
            Quantity = 1,
            Price = 1000
        });

        Console.WriteLine($"Market Items: {Listings.Count}");
    }
}
