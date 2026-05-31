using Intersect.Server.Core.CommandParsing;
using Intersect.Server.Localization;

namespace Intersect.Server.Core.Commands
{

    internal sealed partial class MarketTestCommand : ServerCommand
    {

        public MarketTestCommand() : base(Strings.Commands.Help)
        {
        }

        protected override void HandleValue(ServerContext context, ParserResult result)
        {
            Console.WriteLine($"Market Listings: {MarketService.Listings.Count}");
        }

    }

}
