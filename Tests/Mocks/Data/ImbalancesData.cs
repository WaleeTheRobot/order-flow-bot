
using System.Collections.Generic;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.DataBars.Base;

namespace OrderFlowBot.Tests.Mocks.Data
{
    public static class ImbalancesData
    {
        public static List<ImbalancePrice> GetTestBarBidImbalances()
        {
            return new List<ImbalancePrice>
            {
                new ImbalancePrice { Price = 18557.25, Volume = 36 },
                new ImbalancePrice { Price = 18554.75, Volume = 26 },
                new ImbalancePrice { Price = 18549.75, Volume = 111 }
            };
        }

        public static List<ImbalancePrice> GetTestBarAskImbalances()
        {
            return new List<ImbalancePrice>
            {
                new ImbalancePrice { Price = 18561.00, Volume = 16 },
                new ImbalancePrice { Price = 18559.75, Volume = 20 },
                new ImbalancePrice { Price = 18557.25, Volume = 76 },
                new ImbalancePrice { Price = 18552.25, Volume = 59 }
            };
        }
    }
}
