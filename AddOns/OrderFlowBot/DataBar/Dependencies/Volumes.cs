using System;
using System.Collections.Generic;
using System.Linq;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.DataBar.Dependencies
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
        public bool HasBidVolumeSequencing { get; set; }
        public bool HasAskVolumeSequencing { get; set; }
        public bool HasAskSinglePrint { get; set; }
        public bool HasBidSinglePrint { get; set; }

        public Volumes()
        {
            BidAskVolumes = new List<BidAskVolume>();
        }

        public bool ValidBidAskVolumes()
        {
            return BidAskVolumes.Count > 2;
        }

        public void SetVolumeSequencing(List<BidAskVolume> bidAskVolumes, BarType barType, long totalVolume)
        {
            if (totalVolume < OrderFlowBotProperties.ValidVolumeSequencingMinimumVolume || bidAskVolumes.Count < OrderFlowBotProperties.ValidVolumeSequencing + 1)
            {
                this.HasAskVolumeSequencing = false;
                this.HasBidVolumeSequencing = false;

                return;
            }

            int validVolumeSequencing = OrderFlowBotProperties.ValidVolumeSequencing;
            bool isValidSequence = true;

            if (barType == BarType.Bullish)
            {
                var lastVolumes = bidAskVolumes.Skip(Math.Max(0, bidAskVolumes.Count - validVolumeSequencing)).Take(validVolumeSequencing).Reverse().ToList();

                // Check if the AskVolume is sequentially increasing
                for (int i = 0; i < lastVolumes.Count - 1; i++)
                {
                    if (lastVolumes[i].AskVolume >= lastVolumes[i + 1].AskVolume)
                    {
                        isValidSequence = false;
                        break;
                    }
                }

                this.HasAskVolumeSequencing = isValidSequence;
                this.HasBidVolumeSequencing = false;
            }
            else if (barType == BarType.Bearish)
            {
                var firstVolumes = bidAskVolumes.Take(validVolumeSequencing).ToList();

                // Check if the BidVolume is sequentially increasing
                for (int i = 0; i < firstVolumes.Count - 1; i++)
                {
                    if (firstVolumes[i].BidVolume >= firstVolumes[i + 1].BidVolume)
                    {
                        isValidSequence = false;
                        break;
                    }
                }

                this.HasAskVolumeSequencing = false;
                this.HasBidVolumeSequencing = isValidSequence;
            }
            else
            {
                this.HasAskVolumeSequencing = false;
                this.HasBidVolumeSequencing = false;
            }
        }

        public void SetSinglePrints()
        {
            if (this.BidAskVolumes == null || this.BidAskVolumes.Count == 0)
            {
                return;
            }

            this.HasAskSinglePrint = this.BidAskVolumes.First().AskVolume < 10;
            this.HasBidSinglePrint = this.BidAskVolumes.Last().BidVolume < 10;
        }

        public void SetBidAskPriceVolumeAndVolumeDelta()
        {
            if (this.BidAskVolumes == null || this.BidAskVolumes.Count == 0)
            {
                return;
            }

            for (int i = 0; i < this.BidAskVolumes.Count; i++)
            {
                var bidAskVolume = this.BidAskVolumes[i];
                bidAskVolume.Volume = bidAskVolume.BidVolume + bidAskVolume.AskVolume;
                bidAskVolume.VolumeDelta = bidAskVolume.AskVolume - bidAskVolume.BidVolume;
                this.BidAskVolumes[i] = bidAskVolume;
            }
        }
    }
}
