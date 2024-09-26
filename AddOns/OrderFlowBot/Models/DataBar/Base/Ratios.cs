using NinjaTrader.Custom.AddOns.OrderFlowBot.Utils;
using System.Collections.Generic;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Models.DataBar.Base
{
    public class Ratios
    {
        public double AskRatio { get; set; }
        public double BidRatio { get; set; }

        public void SetRatios(List<BidAskVolume> bidAskVolumes, bool validBidAskVolumes)
        {
            if (!validBidAskVolumes)
                return;

            double secondBottomBid, bottomBid;
            GetBottomBidVolumes(bidAskVolumes, out secondBottomBid, out bottomBid);
            BidRatio = BarUtils.CalculateRatio(secondBottomBid, bottomBid);

            double topAsk, secondTopAsk;
            GetTopAskVolumes(bidAskVolumes, out topAsk, out secondTopAsk);
            AskRatio = BarUtils.CalculateRatio(secondTopAsk, topAsk);
        }

        private void GetBottomBidVolumes(List<BidAskVolume> bidAskVolumes, out double secondBottomBid, out double bottomBid)
        {
            int lastIndex = bidAskVolumes.Count - 1;
            secondBottomBid = bidAskVolumes[lastIndex - 1].BidVolume;
            bottomBid = bidAskVolumes[lastIndex].BidVolume;
        }

        private void GetTopAskVolumes(List<BidAskVolume> bidAskVolumes, out double topAsk, out double secondTopAsk)
        {
            topAsk = bidAskVolumes[0].AskVolume;
            secondTopAsk = bidAskVolumes[1].AskVolume;
        }
    }
}
