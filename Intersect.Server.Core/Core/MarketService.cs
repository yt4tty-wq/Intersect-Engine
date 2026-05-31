using System;
using System.Collections.Generic;
using System.Linq;

namespace Intersect.Server.Core;

public static class MarketService
{
    private static readonly object _lock = new();

    private static readonly List<MarketItem> _listings = new();

    public static IReadOnlyList<MarketItem> Listings
    {
        get
        {
            lock (_lock)
                return _listings.ToList();
        }
    }

    // CREATE LISTING (MAIN ENTRY)
    public static bool CreateListing(Guid sellerId, Guid itemId, int quantity, long price)
    {
        if (quantity <= 0 || price <= 0)
            return false;

        lock (_lock)
        {
            var item = new MarketItem
            {
                ListingId = Guid.NewGuid(),
                SellerId = sellerId,
                ItemId = itemId,
                Quantity = quantity,
                Price = price
            };

            _listings.Add(item);
            return true;
        }
    }

    public static MarketItem? FindListing(Guid listingId)
    {
        lock (_lock)
        {
            return _listings.FirstOrDefault(x => x.ListingId == listingId);
        }
    }

    public static bool RemoveListing(Guid listingId)
    {
        lock (_lock)
        {
            var item = _listings.FirstOrDefault(x => x.ListingId == listingId);

            if (item == null)
                return false;

            _listings.Remove(item);
            return true;
        }
    }

    public static bool TryGetAndRemove(Guid listingId, out MarketItem? item)
    {
        lock (_lock)
        {
            item = _listings.FirstOrDefault(x => x.ListingId == listingId);

            if (item == null)
                return false;

            _listings.Remove(item);
            return true;
        }
    }
}
