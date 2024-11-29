using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Models.DataBars.Base
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
        public double ValueAreaPercentage { get; set; }
        public double ValueAreaHighPrice { get; set; }
        public double ValueAreaLowPrice { get; set; }

        public Volumes(IDataBarConfig config)
        {
            BidAskVolumes = new List<BidAskVolume>();
            ValueAreaPercentage = Math.Round(config.ValueAreaPercentage * 0.01, 2);
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

        // This can be updated to the one NinjaTrader provides if it ever gets created
        public void SetValueArea()
        {
            if (BidAskVolumes == null || BidAskVolumes.Count == 0)
            {
                return;
            }

            var sortedVolumes = BidAskVolumes.OrderBy(v => v.Price).ToList();

            var poc = sortedVolumes.OrderByDescending(v => v.Volume).First();
            PointOfControl = poc.Price;

            long totalVolume = sortedVolumes.Sum(v => v.Volume);
            long valueAreaVolume = (long)(totalVolume * ValueAreaPercentage);
            long currentVolume = poc.Volume;

            int lowerIndex = sortedVolumes.IndexOf(poc);
            int upperIndex = lowerIndex;

            while (currentVolume < valueAreaVolume && (lowerIndex > 0 || upperIndex < sortedVolumes.Count - 1))
            {
                // Ensure the lowerIndex and upperIndex are within valid bounds
                long lowerVolume = (lowerIndex > 0) ? sortedVolumes[lowerIndex - 1].Volume : long.MinValue;
                long upperVolume = (upperIndex < sortedVolumes.Count - 1) ? sortedVolumes[upperIndex + 1].Volume : long.MinValue;

                // If both volumes are invalid, break to avoid an infinite loop
                if (lowerVolume == long.MinValue && upperVolume == long.MinValue)
                {
                    break;
                }

                if (lowerVolume >= upperVolume && lowerIndex > 0)
                {
                    currentVolume += lowerVolume;
                    lowerIndex--;
                }
                else if (upperIndex < sortedVolumes.Count - 1)
                {
                    currentVolume += upperVolume;
                    upperIndex++;
                }
            }

            // Ensure the indices are still within bounds before setting prices
            if (lowerIndex >= 0 && lowerIndex < sortedVolumes.Count)
            {
                ValueAreaLowPrice = sortedVolumes[lowerIndex].Price;
            }

            if (upperIndex >= 0 && upperIndex < sortedVolumes.Count)
            {
                ValueAreaHighPrice = sortedVolumes[upperIndex].Price;
            }
        }
    }
}
