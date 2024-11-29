using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.DataBars.Base;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Utils;
using System;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Models.DataBars
{
    public class DataBar : IReadOnlyDataBar
    {
        public IDataBarDataProvider DataBarDataProvider { get; private set; }
        public BarType BarType { get; private set; }
        public int Time { get; private set; }
        public int BarNumber { get; private set; }

        public Prices Prices { get; private set; }
        public Ratios Ratios { get; private set; }
        public Volumes Volumes { get; private set; }
        public Deltas Deltas { get; private set; }
        public Imbalances Imbalances { get; private set; }
        public CumulativeDeltaBar CumulativeDeltaBar { get; private set; }

        public DataBar(IDataBarConfig config)
        {
            DataBarDataProvider = new DataBarDataProvider();
            Prices = new Prices();
            Ratios = new Ratios();
            Volumes = new Volumes(config);
            Deltas = new Deltas();
            Imbalances = new Imbalances(config);
            CumulativeDeltaBar = new CumulativeDeltaBar();
        }

        public void SetCurrentDataBar(IDataBarDataProvider dataBarDataProvider)
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
            PopulateCumulativeDeltaBar();
        }

        private void SetBarType()
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
            var volumes = DataBarDataProvider.VolumetricBar;

            Volumes.Volume = volumes.TotalVolume;
            Volumes.BuyingVolume = volumes.TotalBuyingVolume;
            Volumes.SellingVolume = volumes.TotalSellingVolume;
            Volumes.PointOfControl = volumes.PointOfControl;
            Volumes.BidAskVolumes = volumes.BidAskVolumes;
            Volumes.SetBidAskPriceVolumeAndVolumeDelta();
            Volumes.SetValueArea();

            if (volumes.BidAskVolumes.Count > 2)
            {
                Imbalances.SetImbalances(volumes.BidAskVolumes);
                Ratios.SetRatios(volumes.BidAskVolumes);
            }
        }

        private void PopulateDeltas()
        {
            var volumes = DataBarDataProvider.VolumetricBar;
            long minDelta = volumes.MinSeenDelta;
            long maxDelta = volumes.MaxSeenDelta;

            Deltas.Delta = volumes.BarDelta;
            Deltas.MinDelta = minDelta;
            Deltas.MaxDelta = maxDelta;
            Deltas.DeltaSh = volumes.DeltaSh;
            Deltas.DeltaSl = volumes.DeltaSl;
            Deltas.CumulativeDelta = volumes.CumulativeDelta;
            Deltas.DeltaPercentage = volumes.DeltaPercentage;

            Deltas.MinMaxDeltaRatio = BarUtils.CalculateRatio(Math.Abs(minDelta), Math.Abs(maxDelta));
            Deltas.MaxMinDeltaRatio = BarUtils.CalculateRatio(Math.Abs(maxDelta), Math.Abs(minDelta));
            Deltas.DeltaChange = volumes.DeltaChange;
        }

        private void PopulateCumulativeDeltaBar()
        {
            CumulativeDeltaBar = DataBarDataProvider.CumulativeDeltaBar;
        }
    }
}
