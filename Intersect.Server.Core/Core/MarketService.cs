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

    // เพิ่มตรงนี้
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
}
