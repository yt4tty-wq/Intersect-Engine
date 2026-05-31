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
}
