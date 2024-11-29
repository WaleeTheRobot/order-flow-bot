using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.DataBars;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.DataBars.Base;
using NinjaTrader.Custom.AddOns.OrderFlowBot.UserInterfaces.Configs;
using NinjaTrader.NinjaScript.BarsTypes;
using NinjaTrader.NinjaScript.Indicators;
using System;
using System.Collections.Generic;
using System.IO;

namespace NinjaTrader.NinjaScript.Strategies
{
    public partial class OrderFlowBot : Strategy
    {
        private DataBarDataProvider _dataBarDataProvider;
        private OrderFlowCumulativeDelta _cumulativeDelta;

        private void InitializeDataBar()
        {
            _dataBarDataProvider = new DataBarDataProvider();
        }

        private IDataBarDataProvider GetDataBarDataProvider(IDataBarConfig config, int barsAgo = 0)
        {
            _dataBarDataProvider.Time = ToTime(Time[barsAgo]);
            _dataBarDataProvider.CurrentBar = CurrentBars[0];
            _dataBarDataProvider.BarsAgo = barsAgo;
            _dataBarDataProvider.High = High[barsAgo];
            _dataBarDataProvider.Low = Low[barsAgo];
            _dataBarDataProvider.Open = Open[barsAgo];
            _dataBarDataProvider.Close = Close[barsAgo];

            VolumetricBarsType volumetricBar = Bars.BarsSeries.BarsType as VolumetricBarsType;
            _dataBarDataProvider.VolumetricBar = PopulateCustomVolumetricBar(volumetricBar, config);

            try
            {
                _dataBarDataProvider.CumulativeDeltaBar = new CumulativeDeltaBar
                {
                    Open = _cumulativeDelta.DeltaOpen[barsAgo],
                    Close = _cumulativeDelta.DeltaClose[barsAgo],
                    High = _cumulativeDelta.DeltaHigh[barsAgo],
                    Low = _cumulativeDelta.DeltaLow[barsAgo],
                };
            }
            catch
            {
                // Fallback
                _dataBarDataProvider.CumulativeDeltaBar = new CumulativeDeltaBar();
            }

            return _dataBarDataProvider;
        }

        private ICustomVolumetricBar PopulateCustomVolumetricBar(VolumetricBarsType volumetricBar, IDataBarConfig config)
        {
            ICustomVolumetricBar customBar = new CustomVolumetricBar();

            double high = _dataBarDataProvider.High;
            double low = _dataBarDataProvider.Low;

            var volumes = volumetricBar.Volumes[_dataBarDataProvider.CurrentBar - _dataBarDataProvider.BarsAgo];

            customBar.TotalVolume = volumes.TotalVolume;
            customBar.TotalBuyingVolume = volumes.TotalBuyingVolume;
            customBar.TotalSellingVolume = volumes.TotalSellingVolume;

            volumes.GetMaximumVolume(null, out double pointOfControl);
            customBar.PointOfControl = pointOfControl;

            // Get bid/ask volume for each price in bar
            List<BidAskVolume> bidAskVolumeList = new List<BidAskVolume>();
            int ticksPerLevel = config.TicksPerLevel;
            int totalLevels = 0;
            int counter = 0;

            while (high >= low)
            {
                if (counter == 0)
                {
                    BidAskVolume bidAskVolume = new BidAskVolume
                    {
                        Price = high,
                        BidVolume = volumes.GetBidVolumeForPrice(high),
                        AskVolume = volumes.GetAskVolumeForPrice(high)
                    };

                    bidAskVolumeList.Add(bidAskVolume);
                }

                if (counter == ticksPerLevel - 1)
                {
                    counter = 0;
                }
                else
                {
                    counter++;
                }

                totalLevels++;
                high -= config.TickSize;
            }

            // Remove the first item if total levels are not divisible by ticksPerLevel and more than 4 levels
            // Sometimes bidAskVolumeList doesn't correlate visually due to an extra level or lack of a level
            // This seems to resolve probably many of the realistic scenarios
            if (totalLevels % ticksPerLevel > 0 && bidAskVolumeList.Count > 4)
            {
                bidAskVolumeList.RemoveAt(0);
            }

            customBar.BidAskVolumes = bidAskVolumeList;

            // Deltas
            customBar.BarDelta = volumes.BarDelta;
            customBar.MinSeenDelta = volumes.MinSeenDelta;
            customBar.MaxSeenDelta = volumes.MaxSeenDelta;
            customBar.DeltaSh = volumes.DeltaSh;
            customBar.DeltaSl = volumes.DeltaSl;
            customBar.CumulativeDelta = volumes.CumulativeDelta;
            customBar.DeltaPercentage = Math.Round(volumes.GetDeltaPercent(), 2);
            customBar.DeltaChange = volumes.BarDelta - volumetricBar.Volumes[_dataBarDataProvider.CurrentBar - _dataBarDataProvider.BarsAgo - 1].BarDelta;

            return customBar;
        }

        private void SetConfigs()
        {
            DataBarConfig.Instance.TicksPerLevel = TicksPerLevel;
            DataBarConfig.Instance.TickSize = TickSize;
            DataBarConfig.Instance.StackedImbalance = StackedImbalance;
            DataBarConfig.Instance.ImbalanceRatio = ImbalanceRatio;
            DataBarConfig.Instance.ImbalanceMinDelta = ImbalanceMinDelta;
            DataBarConfig.Instance.ValueAreaPercentage = ValueAreaPercentage;

            UserInterfaceConfig.Instance.AssetsPath =
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "NinjaTrader 8", "bin", "Custom", "AddOns", "OrderFlowBot", "Assets");
        }
    }
}
