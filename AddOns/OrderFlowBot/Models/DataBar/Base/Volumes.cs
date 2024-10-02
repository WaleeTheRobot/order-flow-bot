using System.Collections.Generic;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Models.DataBar.Base
{
    public struct BidAskVolume
    {
        public double Price;
        public long BidVolume;
        public long AskVolume;
        public long Volume;
        public long VolumeDelta;
    }

    public class Volumes
    {
        public double PointOfControl { get; set; }
        public long Volume { get; set; }
        public long BuyingVolume { get; set; }
        public long SellingVolume { get; set; }
        public List<BidAskVolume> BidAskVolumes { get; set; }
        public double ValueAreaHighPrice { get; set; }
        public double ValueAreaLowPrice { get; set; }

        public Volumes()
        {
            BidAskVolumes = new List<BidAskVolume>();
        }

        public void SetBidAskPriceVolumeAndVolumeDelta()
        {
            if (BidAskVolumes == null || BidAskVolumes.Count == 0)
            {
                return;
            }

            for (int i = 0; i < BidAskVolumes.Count; i++)
            {
                var bidAskVolume = BidAskVolumes[i];
                bidAskVolume.Volume = bidAskVolume.BidVolume + bidAskVolume.AskVolume;
                bidAskVolume.VolumeDelta = bidAskVolume.AskVolume - bidAskVolume.BidVolume;
                BidAskVolumes[i] = bidAskVolume;
            }
        }
    }
}
