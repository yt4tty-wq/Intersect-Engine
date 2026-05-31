using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Intersect.Server.Database.GameData;
using Microsoft.EntityFrameworkCore;

namespace Intersect.Server.Core.Services;

public static class MarketService
{
    private static readonly object _lock = new();

    private static List<AuctionListing> _cache = new();
    private static bool _dirty;

    public static IReadOnlyList<AuctionListing> Listings
    {
        get
        {
            lock (_lock)
                return _cache.ToList();
        }
    }

    #region Load

    public static void Load(GameContext db)
    {
        lock (_lock)
        {
            _cache = db.AuctionListings
                .AsNoTracking()
                .ToList();

            _dirty = false;
        }
    }

    #endregion

    #region Save

    public static void Save(GameContext db)
    {
        lock (_lock)
        {
            if (!_dirty)
                return;

            using var transaction = db.Database.BeginTransaction();

            try
            {
                // โหลดของใน DB
                var existing = db.AuctionListings.ToList();

                // ลบเฉพาะของที่ไม่อยู่ใน cache (sync diff)
                var cacheIds = _cache.Select(x => x.ListingId).ToHashSet();

                var toRemove = existing
                    .Where(x => !cacheIds.Contains(x.ListingId))
                    .ToList();

                if (toRemove.Count > 0)
                    db.AuctionListings.RemoveRange(toRemove);

                // upsert logic
                foreach (var listing in _cache)
                {
                    var exists = existing.FirstOrDefault(x => x.ListingId == listing.ListingId);

                    if (exists == null)
                    {
                        db.AuctionListings.Add(listing);
                    }
                    else
                    {
                        db.Entry(exists).CurrentValues.SetValues(listing);
                    }
                }

                db.SaveChanges();
                transaction.Commit();

                _dirty = false;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }

    #endregion

    #region CRUD

    public static void Add(AuctionListing listing)
    {
        lock (_lock)
        {
            _cache.Add(listing);
            _dirty = true;
        }
    }

    public static bool Remove(Guid listingId)
    {
        lock (_lock)
        {
            var removed = _cache.RemoveAll(x => x.ListingId == listingId) > 0;

            if (removed)
                _dirty = true;

            return removed;
        }
    }

    public static AuctionListing? Get(Guid listingId)
    {
        lock (_lock)
        {
            return _cache.FirstOrDefault(x => x.ListingId == listingId);
        }
    }

    public static List<AuctionListing> GetBySeller(Guid sellerId)
    {
        lock (_lock)
        {
            return _cache
                .Where(x => x.SellerId == sellerId)
                .ToList();
        }
    }

    #endregion

    #region Maintenance

    public static void CleanupExpired(DateTime now)
    {
        lock (_lock)
        {
            var removed = _cache.RemoveAll(x =>
                x.ExpireAt != null && x.ExpireAt <= now);

            if (removed > 0)
                _dirty = true;
        }
    }

    #endregion
}
