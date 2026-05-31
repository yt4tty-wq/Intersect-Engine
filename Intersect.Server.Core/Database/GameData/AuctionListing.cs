using System;
using System.ComponentModel.DataAnnotations;

namespace Intersect.Server.Database;

public class AuctionListing
{
    [Key]
    public Guid ListingId { get; set; }

    public Guid SellerId { get; set; }

    public Guid ItemId { get; set; }

    public int Quantity { get; set; }

    public long Price { get; set; }

    public DateTime CreatedAt { get; set; }
}
