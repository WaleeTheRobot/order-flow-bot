using NinjaTrader.Custom.AddOns.OrderFlowBot.DataBar;
using System.Collections.Generic;

namespace NinjaTrader.NinjaScript.Strategies
{
    public partial class OrderFlowBot : Strategy
    {
        private void SetPreviousAutoVolumeProfileDataBar()
        {
            NinjaTrader.NinjaScript.BarsTypes.VolumetricBarsType volumetricBar = Bars.BarsSeries.BarsType as
                NinjaTrader.NinjaScript.BarsTypes.VolumetricBarsType;

            if (volumetricBar == null)
                return;

            int barAgo = 1;
            double high = High[barAgo];
            double low = Low[barAgo];

            if (volumetricBar == null)
                return;

            OrderFlowBotVolumeProfile volumeProfile = new OrderFlowBotVolumeProfile();

            // Iterate through data from high to low and get the data to update volume profile
            for (double price = high; price >= low; price -= TickSize)
            {
                long bidVolume = volumetricBar.Volumes[CurrentBar - barAgo].GetBidVolumeForPrice(price);
                long askVolume = volumetricBar.Volumes[CurrentBar - barAgo].GetAskVolumeForPrice(price);

                volumeProfile.AddOrUpdateVolume(price, bidVolume + askVolume);
            }

            volumeProfile.SetTotalVolume();
            volumeProfile.CalculateValueArea();

            //_volumeProfileDataBars.Bars.Add(volumeProfile);
        }

        private void SetAutoVolumeProfileDataBar()
        {
            // Combines previous bar up to MaxBarLookBack
            List<OrderFlowBotVolumeProfile> customVolumeProfiles = new List<OrderFlowBotVolumeProfile>();

            //int start = _volumeProfileDataBars.Bars.Count - 1;
            //int stop = Math.Max(0, start - AutoVolumeProfileLookBackBars);

            //for (int i = start; i > stop; i--)
            //{
            //    customVolumeProfiles.Add(_volumeProfileDataBars.Bars[i]);
            //}

            //_volumeProfileDataBar.CombineVolumeProfiles(customVolumeProfiles);
        }
    }
}
