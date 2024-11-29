using NinjaTrader.Custom.AddOns.OrderFlowBot.Utils;
using System.Collections.Generic;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Models.DataBars.Base
{
    public class Ratios
    {
        public double AskRatio { get; set; }
        public double BidRatio { get; set; }

        public void SetRatios(List<BidAskVolume> bidAskVolumes)
        {
            double secondBottomBid, bottomBid;
            SetBottomBidVolumes(bidAskVolumes, out secondBottomBid, out bottomBid);
            BidRatio = BarUtils.CalculateRatio(secondBottomBid, bottomBid);

            double topAsk, secondTopAsk;
            SetTopAskVolumes(bidAskVolumes, out topAsk, out secondTopAsk);
            AskRatio = BarUtils.CalculateRatio(secondTopAsk, topAsk);
        }

        private static void SetBottomBidVolumes(List<BidAskVolume> bidAskVolumes, out double secondBottomBid, out double bottomBid)
        {
            int lastIndex = bidAskVolumes.Count - 1;
            secondBottomBid = bidAskVolumes[lastIndex - 1].BidVolume;
            bottomBid = bidAskVolumes[lastIndex].BidVolume;
        }

        private static void SetTopAskVolumes(List<BidAskVolume> bidAskVolumes, out double topAsk, out double secondTopAsk)
        {
            topAsk = bidAskVolumes[0].AskVolume;
            secondTopAsk = bidAskVolumes[1].AskVolume;
        }
    }
}
