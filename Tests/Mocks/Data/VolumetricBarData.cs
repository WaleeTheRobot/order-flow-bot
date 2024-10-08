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
            TotalVolume = 4392;
            TotalBuyingVolume = 2117;
            TotalSellingVolume = 2275;
            ValueAreaHighPrice = 18692.25;
            ValueAreaLowPrice = 18679.75;
            PointOfControl = 18686;
            BarDelta = -158;
            MinSeenDelta = -207;
            MaxSeenDelta = 189;
            DeltaSh = -208;
            DeltaSl = 0;
            CumulativeDelta = -1106;
            DeltaPercentage = -3.6;
            DeltaChange = 209;
        }

        public static List<BidAskVolume> GetTestBarBidAskVolume()
        {
            return new List<BidAskVolume>
            {
                new BidAskVolume { Price = 18696, BidVolume = 12, AskVolume = 26 },
                new BidAskVolume { Price = 18694.75, BidVolume = 24, AskVolume = 18 },
                new BidAskVolume { Price = 18693.5, BidVolume = 30, AskVolume = 79 },
                new BidAskVolume { Price = 18692.25, BidVolume = 139, AskVolume = 163 },
                new BidAskVolume { Price = 18691, BidVolume = 119, AskVolume = 141 },
                new BidAskVolume { Price = 18689.75, BidVolume = 157, AskVolume = 206 },
                new BidAskVolume { Price = 18688.5, BidVolume = 105, AskVolume = 169 },
                new BidAskVolume { Price = 18687.25, BidVolume = 148, AskVolume = 211 },
                new BidAskVolume { Price = 18686, BidVolume = 229, AskVolume = 211 },
                new BidAskVolume { Price = 18684.75, BidVolume = 250, AskVolume = 123 },
                new BidAskVolume { Price = 18683.5, BidVolume = 139, AskVolume = 61 },
                new BidAskVolume { Price = 18682.25, BidVolume = 116, AskVolume = 120 },
                new BidAskVolume { Price = 18681, BidVolume = 145, AskVolume = 78 },
                new BidAskVolume { Price = 18679.75, BidVolume = 148, AskVolume = 87 },
                new BidAskVolume { Price = 18678.5, BidVolume = 47, AskVolume = 12 },
                new BidAskVolume { Price = 18677.25, BidVolume = 17, AskVolume = 8 },
                new BidAskVolume { Price = 18676, BidVolume = 35, AskVolume = 24 },
                new BidAskVolume { Price = 18674.75, BidVolume = 33, AskVolume = 52 },
                new BidAskVolume { Price = 18673.5, BidVolume = 54, AskVolume = 56 },
                new BidAskVolume { Price = 18672.25, BidVolume = 95, AskVolume = 88 },
                new BidAskVolume { Price = 18671, BidVolume = 82, AskVolume = 46 },
                new BidAskVolume { Price = 18669.75, BidVolume = 90, AskVolume = 90 },
                new BidAskVolume { Price = 18668.5, BidVolume = 47, AskVolume = 30 },
                new BidAskVolume { Price = 18667.25, BidVolume = 14, AskVolume = 15 }
            };
        }
    }
}
