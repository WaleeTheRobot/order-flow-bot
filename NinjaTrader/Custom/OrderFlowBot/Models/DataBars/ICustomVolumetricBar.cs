using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.DataBars.Base;
using System.Collections.Generic;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Models.DataBars
{
    public interface ICustomVolumetricBar
    {
        long TotalVolume { get; set; }
        long TotalBuyingVolume { get; set; }
        long TotalSellingVolume { get; set; }
        double ValueAreaHighPrice { get; set; }
        double ValueAreaLowPrice { get; set; }
        double PointOfControl { get; set; }
        long BarDelta { get; set; }
        long MinSeenDelta { get; set; }
        long MaxSeenDelta { get; set; }
        long DeltaSh { get; set; }
        long DeltaSl { get; set; }
        long CumulativeDelta { get; set; }
        double DeltaPercentage { get; set; }
        double DeltaChange { get; set; }
        List<BidAskVolume> BidAskVolumes { get; set; }
    }
}
