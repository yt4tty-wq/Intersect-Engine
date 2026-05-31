namespace Intersect.Server.Core;

public class MarketItem
{
    public Guid SellerId { get; set; }

    public Guid ItemId { get; set; }

    public int Quantity { get; set; }

    public long Price { get; set; }
}
