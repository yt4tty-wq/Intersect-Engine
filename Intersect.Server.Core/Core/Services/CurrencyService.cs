using System;
using System.Collections.Generic;

namespace Intersect.Server.Core.Services;

public static class CurrencyService
{
    private static readonly Dictionary<Guid, long> _money = new();

    public static bool HasMoney(Guid playerId, long amount)
    {
        return Get(playerId) >= amount;
    }

    public static bool RemoveMoney(Guid playerId, long amount)
    {
        if (!HasMoney(playerId, amount))
            return false;

        _money[playerId] = Get(playerId) - amount;
        return true;
    }

    public static void AddMoney(Guid playerId, long amount)
    {
        _money[playerId] = Get(playerId) + amount;
    }

    public static long Get(Guid playerId)
    {
        if (!_money.ContainsKey(playerId))
            _money[playerId] = 0;

        return _money[playerId];
    }
}
