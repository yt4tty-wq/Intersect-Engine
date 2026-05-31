using System;
using Intersect.Server.Core;

namespace Intersect.Server.Core.Services;

public static class MarketSellService
{
    public static bool TrySellItem(
        Guid playerId,
        Guid itemId,
        int quantity,
        long price)
    {
        if (quantity <= 0 || price <= 0)
            return false;

        // 1. CHECK INVENTORY
        if (!InventoryService.HasItem(playerId, itemId, quantity))
        {
            Console.WriteLine("[MARKET] Not enough items");
            return false;
        }

        // 2. REMOVE ITEM FROM PLAYER
        var removed = InventoryService.RemoveItem(playerId, itemId, quantity);
        if (!removed)
        {
            Console.WriteLine("[MARKET] Failed to remove item");
            return false;
        }

        // 3. CREATE LISTING
        var created = MarketService.CreateListing(
            playerId,
            itemId,
            quantity,
            price
        );

        if (!created)
        {
            // rollback (คืนของ)
            InventoryService.AddItem(playerId, itemId, quantity);
            return false;
        }

        Console.WriteLine("[MARKET] Item listed successfully");
        return true;
    }
}
