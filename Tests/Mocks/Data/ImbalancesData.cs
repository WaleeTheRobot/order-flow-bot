
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
                new ImbalancePrice { Price = 18693.5, Volume = 30 },
                new ImbalancePrice { Price = 18692.25, Volume = 139 },
                new ImbalancePrice { Price = 18682.25, Volume = 116 },
                new ImbalancePrice { Price = 18679.75, Volume = 148 },
                new ImbalancePrice { Price = 18676, Volume = 35 },
                new ImbalancePrice { Price = 18672.25, Volume = 95 },
                new ImbalancePrice { Price = 18669.75, Volume = 90 }
            };
        }

        public static List<ImbalancePrice> GetTestBarAskImbalances()
        {
            return new List<ImbalancePrice>
            {
                new ImbalancePrice { Price = 18689.75, Volume = 206 },
                new ImbalancePrice { Price = 18679.75, Volume = 87 },
                new ImbalancePrice { Price = 18669.75, Volume = 90 },
                new ImbalancePrice { Price = 18668.5, Volume = 30 }
            };
        }
    }
}
