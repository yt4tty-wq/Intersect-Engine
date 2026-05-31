using System;
using System.ComponentModel.DataAnnotations;

namespace Intersect.Server.Database.GameData;

public class AuctionListing
{
    public Guid ListingId { get; set; } = Guid.NewGuid();
    public Guid SellerId { get; set; }
    public Guid ItemId { get; set; }
    public int Quantity { get; set; }
    public long Price { get; set; }
}
