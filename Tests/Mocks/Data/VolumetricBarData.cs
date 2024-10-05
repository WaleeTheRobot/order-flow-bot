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
            TotalVolume = 923;
            TotalBuyingVolume = 454;
            TotalSellingVolume = 469;
            //ValueAreaHighPrice = 0;
            //ValueAreaLowPrice = 0;
            PointOfControl = 18536.25;
            BarDelta = -15;
            MinSeenDelta = -100;
            MaxSeenDelta = 2;
            DeltaSh = 75;
            DeltaSl = -4;
            CumulativeDelta = -3003;
            DeltaPercentage = -1.63;
            DeltaChange = -137;
        }

        // The number in the method name represents the ticks per level.
        // The character before the number represents the same bar data.
        // Single tick per level bar
        public static List<BidAskVolume> GetTestBarA1BidAskVolume()
        {
            return new List<BidAskVolume>
            {
                new BidAskVolume { Price = 18549.75, BidVolume = 0, AskVolume = 3 },
                new BidAskVolume { Price = 18549.5, BidVolume = 2, AskVolume = 7 },
                new BidAskVolume { Price = 18549.25, BidVolume = 2, AskVolume = 3 },
                new BidAskVolume { Price = 18549, BidVolume = 5, AskVolume = 3 },
                new BidAskVolume { Price = 18548.75, BidVolume = 3, AskVolume = 3 },
                new BidAskVolume { Price = 18548.5, BidVolume = 2, AskVolume = 0 },
                new BidAskVolume { Price = 18548.25, BidVolume = 1, AskVolume = 2 },
                new BidAskVolume { Price = 18548, BidVolume = 3, AskVolume = 8 },
                new BidAskVolume { Price = 18547.75, BidVolume = 4, AskVolume = 5 },
                new BidAskVolume { Price = 18547.5, BidVolume = 5, AskVolume = 3 },
                new BidAskVolume { Price = 18547.25, BidVolume = 1, AskVolume = 4 },
                new BidAskVolume { Price = 18547, BidVolume = 2, AskVolume = 0 },
                new BidAskVolume { Price = 18546.75, BidVolume = 2, AskVolume = 4 },
                new BidAskVolume { Price = 18546.5, BidVolume = 2, AskVolume = 4 },
                new BidAskVolume { Price = 18546.25, BidVolume = 5, AskVolume = 6 },
                new BidAskVolume { Price = 18546, BidVolume = 8, AskVolume = 3 },
                new BidAskVolume { Price = 18545.75, BidVolume = 3, AskVolume = 5 },
                new BidAskVolume { Price = 18545.5, BidVolume = 3, AskVolume = 8 },
                new BidAskVolume { Price = 18545.25, BidVolume = 2, AskVolume = 5 },
                new BidAskVolume { Price = 18545, BidVolume = 37, AskVolume = 6 },
                new BidAskVolume { Price = 18544.75, BidVolume = 7, AskVolume = 3 },
                new BidAskVolume { Price = 18544.5, BidVolume = 3, AskVolume = 4 },
                new BidAskVolume { Price = 18544.25, BidVolume = 4, AskVolume = 0 },
                new BidAskVolume { Price = 18544, BidVolume = 5, AskVolume = 9 },
                new BidAskVolume { Price = 18543.75, BidVolume = 7, AskVolume = 5 },
                new BidAskVolume { Price = 18543.5, BidVolume = 7, AskVolume = 5 },
                new BidAskVolume { Price = 18543.25, BidVolume = 3, AskVolume = 6 },
                new BidAskVolume { Price = 18543, BidVolume = 31, AskVolume = 4 },
                new BidAskVolume { Price = 18542.75, BidVolume = 4, AskVolume = 5 },
                new BidAskVolume { Price = 18542.5, BidVolume = 7, AskVolume = 7 },
                new BidAskVolume { Price = 18542.25, BidVolume = 2, AskVolume = 4 },
                new BidAskVolume { Price = 18542, BidVolume = 8, AskVolume = 13 },
                new BidAskVolume { Price = 18541.75, BidVolume = 12, AskVolume = 6 },
                new BidAskVolume { Price = 18541.5, BidVolume = 12, AskVolume = 11 },
                new BidAskVolume { Price = 18541.25, BidVolume = 13, AskVolume = 13 },
                new BidAskVolume { Price = 18541, BidVolume = 4, AskVolume = 6 },
                new BidAskVolume { Price = 18540.75, BidVolume = 8, AskVolume = 7 },
                new BidAskVolume { Price = 18540.5, BidVolume = 8, AskVolume = 7 },
                new BidAskVolume { Price = 18540.25, BidVolume = 13, AskVolume = 5 },
                new BidAskVolume { Price = 18540, BidVolume = 7, AskVolume = 5 },
                new BidAskVolume { Price = 18539.75, BidVolume = 10, AskVolume = 9 },
                new BidAskVolume { Price = 18539.5, BidVolume = 8, AskVolume = 9 },
                new BidAskVolume { Price = 18539.25, BidVolume = 3, AskVolume = 3 },
                new BidAskVolume { Price = 18539, BidVolume = 9, AskVolume = 6 },
                new BidAskVolume { Price = 18538.75, BidVolume = 5, AskVolume = 6 },
                new BidAskVolume { Price = 18538.5, BidVolume = 11, AskVolume = 6 },
                new BidAskVolume { Price = 18538.25, BidVolume = 2, AskVolume = 11 },
                new BidAskVolume { Price = 18538, BidVolume = 4, AskVolume = 14 },
                new BidAskVolume { Price = 18537.75, BidVolume = 5, AskVolume = 14 },
                new BidAskVolume { Price = 18537.5, BidVolume = 4, AskVolume = 9 },
                new BidAskVolume { Price = 18537.25, BidVolume = 7, AskVolume = 8 },
                new BidAskVolume { Price = 18537, BidVolume = 8, AskVolume = 14 },
                new BidAskVolume { Price = 18536.75, BidVolume = 8, AskVolume = 16 },
                new BidAskVolume { Price = 18536.5, BidVolume = 12, AskVolume = 10 },
                new BidAskVolume { Price = 18536.25, BidVolume = 31, AskVolume = 16 },
                new BidAskVolume { Price = 18536, BidVolume = 16, AskVolume = 24 },
                new BidAskVolume { Price = 18535.75, BidVolume = 16, AskVolume = 13 },
                new BidAskVolume { Price = 18535.5, BidVolume = 11, AskVolume = 10 },
                new BidAskVolume { Price = 18535.25, BidVolume = 6, AskVolume = 9 },
                new BidAskVolume { Price = 18535, BidVolume = 4, AskVolume = 12 },
                new BidAskVolume { Price = 18534.75, BidVolume = 8, AskVolume = 5 },
                new BidAskVolume { Price = 18534.5, BidVolume = 9, AskVolume = 7 },
                new BidAskVolume { Price = 18534.25, BidVolume = 4, AskVolume = 5 },
                new BidAskVolume { Price = 18534, BidVolume = 2, AskVolume = 2 },
                new BidAskVolume { Price = 18533.75, BidVolume = 6, AskVolume = 3 },
                new BidAskVolume { Price = 18533.5, BidVolume = 1, AskVolume = 6 },
                new BidAskVolume { Price = 18533.25, BidVolume = 1, AskVolume = 0 },
                new BidAskVolume { Price = 18533, BidVolume = 1, AskVolume = 0 }
            };
        }

        // Three ticks per level bar based on the single tick bar data
        public static List<BidAskVolume> GetTestBarA3BidAskVolume()
        {
            return new List<BidAskVolume>
            {
                new BidAskVolume { Price = 18549, BidVolume = 10, AskVolume = 9 },
                new BidAskVolume { Price = 18548.25, BidVolume = 6, AskVolume = 10 },
                new BidAskVolume { Price = 18547.5, BidVolume = 10, AskVolume = 12 },
                new BidAskVolume { Price = 18546.75, BidVolume = 6, AskVolume = 8 },
                new BidAskVolume { Price = 18546, BidVolume = 16, AskVolume = 14 },
                new BidAskVolume { Price = 18545.25, BidVolume = 42, AskVolume = 19 },
                new BidAskVolume { Price = 18544.5, BidVolume = 14, AskVolume = 7 },
                new BidAskVolume { Price = 18543.75, BidVolume = 19, AskVolume = 19 },
                new BidAskVolume { Price = 18543, BidVolume = 38, AskVolume = 15 },
                new BidAskVolume { Price = 18542.25, BidVolume = 17, AskVolume = 24 },
                new BidAskVolume { Price = 18541.5, BidVolume = 37, AskVolume = 30 },
                new BidAskVolume { Price = 18540.75, BidVolume = 20, AskVolume = 20 },
                new BidAskVolume { Price = 18540, BidVolume = 30, AskVolume = 19 },
                new BidAskVolume { Price = 18539.25, BidVolume = 20, AskVolume = 18 },
                new BidAskVolume { Price = 18538.5, BidVolume = 18, AskVolume = 23 },
                new BidAskVolume { Price = 18537.75, BidVolume = 13, AskVolume = 37 },
                new BidAskVolume { Price = 18537, BidVolume = 23, AskVolume = 38 },
                new BidAskVolume { Price = 18536.25, BidVolume = 59, AskVolume = 50 },
                new BidAskVolume { Price = 18535.5, BidVolume = 33, AskVolume = 32 },
                new BidAskVolume { Price = 18534.75, BidVolume = 21, AskVolume = 24 },
                new BidAskVolume { Price = 18534, BidVolume = 12, AskVolume = 10 },
                new BidAskVolume { Price = 18533.25, BidVolume = 3, AskVolume = 6 }
            };
        }

        // Three ticks per level bar's bid imbalances
        public static List<BidAskVolume> GetTestBarA3BidAskVolumeBidImbalances()
        {
            return new List<BidAskVolume>
            {
                new BidAskVolume { Price = 18545.25, BidVolume = 42, AskVolume = 0 },
                new BidAskVolume { Price = 18543.75, BidVolume = 19, AskVolume = 0 },
                new BidAskVolume { Price = 18543, BidVolume = 38, AskVolume = 0 },
                new BidAskVolume { Price = 18541.5, BidVolume = 37, AskVolume = 0 },
                new BidAskVolume { Price = 18540, BidVolume = 30, AskVolume = 0 },
                new BidAskVolume { Price = 18536.25, BidVolume = 59, AskVolume = 0 }
            };
        }

        // Three ticks per level bar's ask imbalances
        public static List<BidAskVolume> GetTestBarA3BidAskVolumeAskImbalances()
        {
            return new List<BidAskVolume>
            {
                new BidAskVolume { Price = 18541.5, BidVolume = 30, AskVolume = 0 },
                new BidAskVolume { Price = 18538.5, BidVolume = 23, AskVolume = 0 },
                new BidAskVolume { Price = 18537.75, BidVolume = 37, AskVolume = 0 },
                new BidAskVolume { Price = 18536.25, BidVolume = 50, AskVolume = 0 },
                new BidAskVolume { Price = 18535.5, BidVolume = 32, AskVolume = 0 },
                new BidAskVolume { Price = 18534.75, BidVolume = 24, AskVolume = 0 }
            };
        }

        // Five ticks per level bar with ask stacked imbalances
        public static List<BidAskVolume> GetTestBarB5BidAskVolume()
        {
            return new List<BidAskVolume>
            {
                new BidAskVolume { Price = 18549.75, BidVolume = 40, AskVolume = 91 },
                new BidAskVolume { Price = 18548.5, BidVolume = 78, AskVolume = 78 },
                new BidAskVolume { Price = 18547.25, BidVolume = 90, AskVolume = 95 },
                new BidAskVolume { Price = 18546, BidVolume = 94, AskVolume = 100 },
                new BidAskVolume { Price = 18544.75, BidVolume = 113, AskVolume = 92 },
                new BidAskVolume { Price = 18543.5, BidVolume = 99, AskVolume = 73 },
                new BidAskVolume { Price = 18542.25, BidVolume = 72, AskVolume = 59 },
                new BidAskVolume { Price = 18541, BidVolume = 54, AskVolume = 59 },
                new BidAskVolume { Price = 18539.75, BidVolume = 63, AskVolume = 57 },
                new BidAskVolume { Price = 18538.5, BidVolume = 44, AskVolume = 51 },
                new BidAskVolume { Price = 18537.25, BidVolume = 36, AskVolume = 68 },
                new BidAskVolume { Price = 18536, BidVolume = 91, AskVolume = 81 },
                new BidAskVolume { Price = 18534.75, BidVolume = 33, AskVolume = 48 },
                new BidAskVolume { Price = 18533.5, BidVolume = 16, AskVolume = 44 },
                new BidAskVolume { Price = 18532.25, BidVolume = 13, AskVolume = 23 },
                new BidAskVolume { Price = 18531, BidVolume = 9, AskVolume = 12 },
                new BidAskVolume { Price = 18529.75, BidVolume = 15, AskVolume = 35 },
                new BidAskVolume { Price = 18528.5, BidVolume = 14, AskVolume = 15 },
                new BidAskVolume { Price = 18527.25, BidVolume = 37, AskVolume = 31 },
                new BidAskVolume { Price = 18526, BidVolume = 42, AskVolume = 46 },
                new BidAskVolume { Price = 18524.75, BidVolume = 45, AskVolume = 64 },
                new BidAskVolume { Price = 18523.5, BidVolume = 45, AskVolume = 54 },
                new BidAskVolume { Price = 18522.25, BidVolume = 40, AskVolume = 59 },
                new BidAskVolume { Price = 18521, BidVolume = 35, AskVolume = 25 },
                new BidAskVolume { Price = 18519.75, BidVolume = 32, AskVolume = 9 },
                new BidAskVolume { Price = 18518.5, BidVolume = 41, AskVolume = 20 },
                new BidAskVolume { Price = 18517.25, BidVolume = 17, AskVolume = 20 },
                new BidAskVolume { Price = 18516, BidVolume = 15, AskVolume = 17 },
                new BidAskVolume { Price = 18514.75, BidVolume = 22, AskVolume = 4 }
            };
        }

        // Five ticks per level bar's ask stacked imbalances
        public static List<BidAskVolume> GetTestBarB5BidAskVolumeAskStackedImbalances()
        {
            return new List<BidAskVolume>
            {
                new BidAskVolume { Price = 18536, BidVolume = 81, AskVolume = 0 },
                new BidAskVolume { Price = 18534.75, BidVolume = 48, AskVolume = 0 },
                new BidAskVolume { Price = 18533.5, BidVolume = 44, AskVolume = 0 },
                new BidAskVolume { Price = 18532.25, BidVolume = 23, AskVolume = 0 }
            };
        }

        // Five ticks per level bar with bid stacked imbalances
        public static List<BidAskVolume> GetTestBarC5BidAskVolume()
        {
            return new List<BidAskVolume>
            {
                new BidAskVolume { Price = 18459.75, BidVolume = 9, AskVolume = 12 },
                new BidAskVolume { Price = 18458.5, BidVolume = 19, AskVolume = 12 },
                new BidAskVolume { Price = 18457.25, BidVolume = 13, AskVolume = 8 },
                new BidAskVolume { Price = 18456, BidVolume = 26, AskVolume = 10 },
                new BidAskVolume { Price = 18454.75, BidVolume = 34, AskVolume = 10 },
                new BidAskVolume { Price = 18453.5, BidVolume = 49, AskVolume = 40 },
                new BidAskVolume { Price = 18452.25, BidVolume = 35, AskVolume = 47 },
                new BidAskVolume { Price = 18451, BidVolume = 66, AskVolume = 64 },
                new BidAskVolume { Price = 18449.75, BidVolume = 109, AskVolume = 94 },
                new BidAskVolume { Price = 18448.5, BidVolume = 145, AskVolume = 132 },
                new BidAskVolume { Price = 18447.25, BidVolume = 103, AskVolume = 101 },
                new BidAskVolume { Price = 18446, BidVolume = 103, AskVolume = 96 },
                new BidAskVolume { Price = 18444.75, BidVolume = 96, AskVolume = 56 },
                new BidAskVolume { Price = 18443.5, BidVolume = 73, AskVolume = 42 },
                new BidAskVolume { Price = 18442.25, BidVolume = 50, AskVolume = 19 },
                new BidAskVolume { Price = 18441, BidVolume = 44, AskVolume = 28 },
                new BidAskVolume { Price = 18439.75, BidVolume = 68, AskVolume = 31 },
                new BidAskVolume { Price = 18438.5, BidVolume = 58, AskVolume = 12 },
                new BidAskVolume { Price = 18437.25, BidVolume = 61, AskVolume = 30 },
                new BidAskVolume { Price = 18436, BidVolume = 44, AskVolume = 45 },
                new BidAskVolume { Price = 18434.75, BidVolume = 20, AskVolume = 6 }
            };
        }

        // Five ticks per level bar's bid stacked imbalances
        public static List<BidAskVolume> GetTestBarC5BidAskVolumeBidStackedImbalances()
        {
            return new List<BidAskVolume>
            {
                new BidAskVolume { Price = 18456, BidVolume = 26, AskVolume = 0 },
                new BidAskVolume { Price = 18454.75, BidVolume = 34, AskVolume = 0 },
                new BidAskVolume { Price = 18453.5, BidVolume = 49, AskVolume = 0 },
                new BidAskVolume { Price = 18441, BidVolume = 44, AskVolume = 0 },
                new BidAskVolume { Price = 18439.75, BidVolume = 68, AskVolume = 0 },
                new BidAskVolume { Price = 18438.5, BidVolume = 58, AskVolume = 0 },
                new BidAskVolume { Price = 18437.25, BidVolume = 61, AskVolume = 0 }
            };
        }
    }
}
