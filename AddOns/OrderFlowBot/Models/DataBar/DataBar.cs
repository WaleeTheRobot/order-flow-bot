using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;
using NinjaTrader.Custom.AddOns.OrderFlowBot.DataBarConfigs;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.DataBar.Base;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Utils;
using System;
using System.Collections.Generic;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Models.DataBar
{
    public class DataBar
    {
        public DataBarDataProvider DataBarDataProvider { get; set; }
        public BarType BarType { get; set; }
        public int Time { get; set; }
        public int BarNumber { get; set; }

        public Prices Prices { get; set; }
        public Ratios Ratios { get; set; }
        public Volumes Volumes { get; set; }
        public Deltas Deltas { get; set; }
        public Imbalances Imbalances { get; set; }

        public DataBar()
        {
            DataBarDataProvider = new DataBarDataProvider();
            Prices = new Prices();
            Ratios = new Ratios();
            Volumes = new Volumes();
            Deltas = new Deltas();
            Imbalances = new Imbalances();
        }

        public void SetCurrentDataBar(DataBarDataProvider dataBarDataProvider)
        {
            DataBarDataProvider = dataBarDataProvider;

            if (DataBarDataProvider.VolumetricBar == null)
            {
                return;
            }

            PopulateBasic();
            PopulatePrices();
            PopulateVolumeAndImbalances();
            PopulateDeltas();
        }

        public void SetBarType()
        {
            if (Prices.Open < Prices.Close)
            {
                BarType = BarType.Bullish;
                return;
            }

            if (Prices.Open > Prices.Close)
            {
                BarType = BarType.Bearish;
                return;
            }

            BarType = BarType.Flat;
        }

        private void PopulateBasic()
        {
            Time = DataBarDataProvider.Time;
            BarNumber = DataBarDataProvider.CurrentBar - DataBarDataProvider.BarsAgo;
        }

        private void PopulatePrices()
        {
            Prices.High = DataBarDataProvider.High;
            Prices.Low = DataBarDataProvider.Low;
            Prices.Open = DataBarDataProvider.Open;
            Prices.Close = DataBarDataProvider.Close;
            SetBarType();
        }

        private void PopulateVolumeAndImbalances()
        {
            double high = Prices.High;
            double low = Prices.Low;
            var volumes = DataBarDataProvider.VolumetricBar.Volumes[DataBarDataProvider.CurrentBar - DataBarDataProvider.BarsAgo];

            Volumes.Volume = volumes.TotalVolume;
            Volumes.BuyingVolume = volumes.TotalBuyingVolume;
            Volumes.SellingVolume = volumes.TotalSellingVolume;
            Volumes.ValueAreaHighPrice = volumes.ValueAreaHighPrice;
            Volumes.ValueAreaLowPrice = volumes.ValueAreaLowPrice;

            double pointOfControl;
            volumes.GetMaximumVolume(null, out pointOfControl);
            Volumes.PointOfControl = pointOfControl;

            // Get bid/ask volume for each price in bar
            List<BidAskVolume> bidAskVolumeList = new List<BidAskVolume>();

            int counter = 0;

            while (high >= low)
            {
                // Values are the same up to TicksPerLevel so we just need to and the initial and skip until the next interval
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

                if (counter == DataBarConfig.Instance.TicksPerLevel - 1)
                {
                    counter = 0;
                }
                else
                {
                    counter++;
                }

                high -= DataBarConfig.Instance.TickSize;
            }

            Volumes.BidAskVolumes = bidAskVolumeList;
            Volumes.SetBidAskPriceVolumeAndVolumeDelta();
            Imbalances.SetImbalances(bidAskVolumeList, Volumes.ValidBidAskVolumes());
            Ratios.SetRatios(bidAskVolumeList, Volumes.ValidBidAskVolumes());
        }

        private void PopulateDeltas()
        {
            var volumes = DataBarDataProvider.VolumetricBar.Volumes[DataBarDataProvider.CurrentBar - DataBarDataProvider.BarsAgo];
            long minDelta = volumes.MinSeenDelta;
            long maxDelta = volumes.MaxSeenDelta;

            Deltas.Delta = volumes.BarDelta;
            Deltas.MinDelta = minDelta;
            Deltas.MaxDelta = maxDelta;
            Deltas.DeltaSh = volumes.DeltaSh;
            Deltas.DeltaSl = volumes.DeltaSl;
            Deltas.CumulativeDelta = volumes.CumulativeDelta;
            Deltas.DeltaPercentage = Math.Round(volumes.GetDeltaPercent(), 2);

            Deltas.MinMaxDeltaRatio = BarUtils.CalculateRatio(Math.Abs(minDelta), Math.Abs(maxDelta));
            Deltas.MaxMinDeltaRatio = BarUtils.CalculateRatio(Math.Abs(maxDelta), Math.Abs(minDelta));
            Deltas.DeltaChange = volumes.BarDelta - DataBarDataProvider.VolumetricBar.Volumes[DataBarDataProvider.CurrentBar - DataBarDataProvider.BarsAgo - 1].BarDelta;
        }
    }
}
