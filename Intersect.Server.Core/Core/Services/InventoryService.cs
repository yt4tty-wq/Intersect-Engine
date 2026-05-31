using System;
using System.Collections.Generic;
using System.Linq;

namespace Intersect.Server.Core.Services;

public static class InventoryService
{
    // playerId -> (itemId -> quantity)
    private static readonly Dictionary<Guid, Dictionary<Guid, int>> _inventories = new();

    private static Dictionary<Guid, int> GetInventory(Guid playerId)
    {
        if (!_inventories.ContainsKey(playerId))
            _inventories[playerId] = new Dictionary<Guid, int>();

        return _inventories[playerId];
    }

    public static bool HasItem(Guid playerId, Guid itemId, int amount)
    {
        var inv = GetInventory(playerId);

        return inv.TryGetValue(itemId, out var qty) && qty >= amount;
    }

    public static bool RemoveItem(Guid playerId, Guid itemId, int amount)
    {
        var inv = GetInventory(playerId);

        if (!inv.TryGetValue(itemId, out var qty) || qty < amount)
            return false;

        qty -= amount;

        if (qty <= 0)
            inv.Remove(itemId);
        else
            inv[itemId] = qty;

        return true;
    }

    public static void AddItem(Guid playerId, Guid itemId, int amount)
    {
        var inv = GetInventory(playerId);

        if (!inv.ContainsKey(itemId))
            inv[itemId] = 0;

        inv[itemId] += amount;
    }
}
