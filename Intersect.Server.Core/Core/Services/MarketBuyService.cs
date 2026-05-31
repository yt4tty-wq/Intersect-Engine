using System;
using Intersect.Server.Core;

namespace Intersect.Server.Core.Services;

public static class MarketBuyService
{
    public static bool TryBuyItem(Guid buyerId, Guid listingId)
    {
        // 1. หา listing
        var listing = MarketService.FindListing(listingId);

        if (listing == null)
        {
            Console.WriteLine("[MARKET] Listing not found");
            return false;
        }

        // 2. ป้องกันซื้อของตัวเอง
        if (listing.SellerId == buyerId)
        {
            Console.WriteLine("[MARKET] Cannot buy your own item");
            return false;
        }

        // 3. ตรวจสอบเงิน
        if (!CurrencyService.HasMoney(buyerId, listing.Price))
        {
            Console.WriteLine("[MARKET] Not enough money");
            return false;
        }

        // 4. หักเงิน
        if (!CurrencyService.RemoveMoney(buyerId, listing.Price))
        {
            Console.WriteLine("[MARKET] Failed to deduct money");
            return false;
        }

        // 5. ส่งของเข้ากระเป๋า
        InventoryService.AddItem(
            buyerId,
            listing.ItemId,
            listing.Quantity
        );

        // 6. ลบ listing
        MarketService.RemoveListing(listingId);

        Console.WriteLine("[MARKET] Purchase completed");

        return true;
    }
}
