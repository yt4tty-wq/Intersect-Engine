using System;
using System.Collections.Generic;

namespace Intersect.Server.Core;

public static class MarketService
{
    public static List<MarketItem> Listings { get; } = new();

    public static void AddTestItem()
    {
        AddListing(new MarketItem()
        {
            SellerId = Guid.Empty,
            ItemId = Guid.Empty,
            Quantity = 1,
            Price = 1000
        });
    }

    public static void AddListing(MarketItem item)
    {
        Listings.Add(item);

        Console.WriteLine($"[MARKET] Added. Total: {Listings.Count}");
    }

    public static List<MarketItem> GetListings()
    {
        return Listings;
    }

    public static void RemoveListing(MarketItem item)
    {
        Listings.Remove(item);

        Console.WriteLine($"[MARKET] Removed. Total: {Listings.Count}");
    }

    public static bool CreateListing(
        Guid sellerId,
        Guid itemId,
        int quantity,
        long price
    )
    {
        AddListing(new MarketItem()
        {
            SellerId = sellerId,
            ItemId = itemId,
            Quantity = quantity,
            Price = price
        });

        return true;
    }

    public static MarketItem? FindListing(Guid listingId)
    {
        return Listings.Find(x => x.ListingId == listingId);
    }

    public static bool RemoveListing(Guid listingId)
    {
        var listing = FindListing(listingId);

        if (listing == null)
        {
            return false;
        }

        Listings.Remove(listing);

        Console.WriteLine($"[MARKET] Removed Listing {listingId}");

        return true;
    }
}
