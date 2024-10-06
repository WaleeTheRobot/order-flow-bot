using System.Collections.Generic;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.DataBars.Base;

namespace OrderFlowBot.Tests.Mocks.Data
{
    public static class VolumetricBarData
    {
        public static long TotalVolume { get; set; }
        public static long TotalBuyingVolume { get; set; }
        public static long TotalSellingVolume { get; set; }
        public static double ValueAreaHighPrice { get; set; }
        public static double ValueAreaLowPrice { get; set; }
        public static double PointOfControl { get; set; }
        public static long BarDelta { get; set; }
        public static long MinSeenDelta { get; set; }
        public static long MaxSeenDelta { get; set; }
        public static long DeltaSh { get; set; }
        public static long DeltaSl { get; set; }
        public static long CumulativeDelta { get; set; }
        public static double DeltaPercentage { get; set; }
        public static double DeltaChange { get; set; }

        static VolumetricBarData()
        {
            TotalVolume = 1173;
            TotalBuyingVolume = 587;
            TotalSellingVolume = 586;
            //ValueAreaHighPrice = 0;
            //ValueAreaLowPrice = 0;
            PointOfControl = 18533.50;
            BarDelta = 1;
            MinSeenDelta = -90;
            MaxSeenDelta = 95;
            DeltaSh = 3;
            DeltaSl = 52;
            CumulativeDelta = -2339;
            DeltaPercentage = 0.09;
            DeltaChange = -58;
        }

        public static List<BidAskVolume> GetTestBarBidAskVolume()
        {
            return new List<BidAskVolume>
            {
                new BidAskVolume { Price = 18563.5, BidVolume = 3, AskVolume = 4 },
                new BidAskVolume { Price = 18562.25, BidVolume = 3, AskVolume = 14 },
                new BidAskVolume { Price = 18561, BidVolume = 6, AskVolume = 16 },
                new BidAskVolume { Price = 18559.75, BidVolume = 3, AskVolume = 20 },
                new BidAskVolume { Price = 18558.5, BidVolume = 9, AskVolume = 20 },
                new BidAskVolume { Price = 18557.25, BidVolume = 36, AskVolume = 76 },
                new BidAskVolume { Price = 18556, BidVolume = 19, AskVolume = 14 },
                new BidAskVolume { Price = 18554.75, BidVolume = 26, AskVolume = 49 },
                new BidAskVolume { Price = 18553.5, BidVolume = 72, AskVolume = 104 },
                new BidAskVolume { Price = 18552.25, BidVolume = 81, AskVolume = 59 },
                new BidAskVolume { Price = 18551, BidVolume = 28, AskVolume = 19 },
                new BidAskVolume { Price = 18549.75, BidVolume = 111, AskVolume = 6 }
            };
        }
    }
}
