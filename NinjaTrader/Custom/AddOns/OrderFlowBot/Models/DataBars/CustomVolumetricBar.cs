using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.DataBars.Base;
using System.Collections.Generic;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Models.DataBars
{
    public class CustomVolumetricBar : ICustomVolumetricBar
    {
        public long TotalVolume { get; set; }
        public long TotalBuyingVolume { get; set; }
        public long TotalSellingVolume { get; set; }
        public double ValueAreaHighPrice { get; set; }
        public double ValueAreaLowPrice { get; set; }
        public double PointOfControl { get; set; }
        public long BarDelta { get; set; }
        public long MinSeenDelta { get; set; }
        public long MaxSeenDelta { get; set; }
        public long DeltaSh { get; set; }
        public long DeltaSl { get; set; }
        public long CumulativeDelta { get; set; }
        public double DeltaPercentage { get; set; }
        public double DeltaChange { get; set; }
        public List<BidAskVolume> BidAskVolumes { get; set; }

        public CustomVolumetricBar()
        {
            BidAskVolumes = new List<BidAskVolume>();
        }
    }
}
