using NinjaTrader.Custom.AddOns.OrderFlowBot.DataBar.Dependencies;
using NinjaTrader.NinjaScript.AddOns.OrderFlowBot;
using System;
using System.Collections.Generic;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.DataBar
{
    public struct OrderFlowBotDataBarConfigValues
    {
        public double TickSize { get; set; }
        public double ImbalanceRatio { get; set; }
        public int StackedImbalance { get; set; }
        public long ValidImbalanceVolume { get; set; }
        public double ValidExhaustionRatio { get; set; }
        public double ValidAbsorptionRatio { get; set; }
        public int ValidVolumeSequencing { get; set; }
        public long ValidVolumeSequencingMinimumVolume { get; set; }
    }

    public static class OrderFlowBotDataBarConfig
    {
        public static double TickSize { get; private set; }
        public static double ImbalanceRatio { get; private set; }
        public static int StackedImbalance { get; private set; }
        public static long ValidImbalanceVolume { get; private set; }
        public static double ValidExhaustionRatio { get; private set; }
        public static double ValidAbsorptionRatio { get; private set; }
        public static int ValidVolumeSequencing { get; private set; }
        public static long ValidVolumeSequencingMinimumVolume { get; private set; }

        public static void Initialize(OrderFlowBotDataBarConfigValues config)
        {
            TickSize = config.TickSize;
            ImbalanceRatio = config.ImbalanceRatio;
            StackedImbalance = config.StackedImbalance;
            ValidImbalanceVolume = config.ValidImbalanceVolume;
            ValidExhaustionRatio = config.ValidExhaustionRatio;
            ValidAbsorptionRatio = config.ValidAbsorptionRatio;
            ValidVolumeSequencing = config.ValidVolumeSequencing;
            ValidVolumeSequencingMinimumVolume = config.ValidVolumeSequencingMinimumVolume;
        }
    }

    public class OrderFlowDataBarBase
    {
        public NinjaTrader.NinjaScript.BarsTypes.VolumetricBarsType VolumetricBar;
        public int Time { get; set; }
        public int CurrentBar { get; set; }
        public int BarsAgo { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Open { get; set; }
        public double Close { get; set; }

        public OrderFlowDataBarBase()
        {
            VolumetricBar = null;
            Time = 0;
            CurrentBar = 0;
            BarsAgo = 0;
            High = 0;
            Low = 0;
            Open = 0;
            Close = 0;
        }
    }

    public class OrderFlowBotDataBars
    {
        private OrderFlowDataBarBase _baseBar { get; set; }
        public List<OrderFlowBotDataBar> Bars { get; set; }
        public OrderFlowBotDataBar Bar { get; set; }

        public OrderFlowBotDataBars(OrderFlowBotDataBarConfigValues configValues)
        {
            OrderFlowBotDataBarConfig.Initialize(configValues);

            _baseBar = new OrderFlowDataBarBase();
            Bars = new List<OrderFlowBotDataBar>();
            Bar = new OrderFlowBotDataBar();
        }

        public void SetOrderFlowDataBarBase(OrderFlowDataBarBase baseBar)
        {
            _baseBar = baseBar;
        }

        public void SetCurrentDataBar()
        {
            if (_baseBar.VolumetricBar == null)
            {
                return;
            }

            PopulateBasic();
            PopulatePrices();
            PopulateVolumeAndImbalances();
            PopulateDeltas();
        }

        public void UpdateDataBars()
        {
            SetCurrentDataBar();
            Bars.Add(Bar);

            Bar = new OrderFlowBotDataBar();
        }

        private void PopulateBasic()
        {
            Bar.Time = _baseBar.Time;
            Bar.BarNumber = _baseBar.CurrentBar - _baseBar.BarsAgo;
        }

        private void PopulatePrices()
        {
            Bar.Prices.High = _baseBar.High;
            Bar.Prices.Low = _baseBar.Low;
            Bar.Prices.Open = _baseBar.Open;
            Bar.Prices.Close = _baseBar.Close;
            Bar.SetBarType();
        }

        private void PopulateVolumeAndImbalances()
        {
            double high = Bar.Prices.High;
            double low = Bar.Prices.Low;
            var volumes = _baseBar.VolumetricBar.Volumes[_baseBar.CurrentBar - _baseBar.BarsAgo];

            Bar.Volumes.Volume = volumes.TotalVolume;
            Bar.Volumes.BuyingVolume = volumes.TotalBuyingVolume;
            Bar.Volumes.SellingVolume = volumes.TotalSellingVolume;
            // Property that may change in the future and not currently supported. Uncommented for now.
            //Bar.Volumes.ValueAreaHighPrice = volumes.ValueAreaHighPrice;
            //Bar.Volumes.ValueAreaLowPrice = volumes.ValueAreaLowPrice;

            double pointOfControl;
            volumes.GetMaximumVolume(null, out pointOfControl);
            Bar.Volumes.PointOfControl = pointOfControl;

            // Get bid/ask volume for each price in bar
            List<BidAskVolume> bidAskVolumeList = new List<BidAskVolume>();

            while (high >= low)
            {
                BidAskVolume bidAskVolume = new BidAskVolume
                {
                    Price = high,
                    BidVolume = volumes.GetBidVolumeForPrice(high),
                    AskVolume = volumes.GetAskVolumeForPrice(high)
                };

                bidAskVolumeList.Add(bidAskVolume);

                high -= OrderFlowBotDataBarConfig.TickSize;
            }

            Bar.Volumes.BidAskVolumes = bidAskVolumeList;
            Bar.Volumes.SetVolumeSequencing(bidAskVolumeList, Bar.BarType, Bar.Volumes.Volume);
            Bar.Volumes.SetSinglePrints();
            Bar.Volumes.SetBidAskPriceVolumeAndVolumeDelta();
            Bar.Imbalances.SetImbalances(bidAskVolumeList, Bar.Volumes.ValidBidAskVolumes());
            Bar.Ratios.SetLastRatioPrices(Bars);
            Bar.Ratios.SetRatios(bidAskVolumeList, Bar.Volumes.ValidBidAskVolumes(), Bar.BarType);
        }

        private void PopulateDeltas()
        {
            var volumes = _baseBar.VolumetricBar.Volumes[_baseBar.CurrentBar - _baseBar.BarsAgo];
            long minDelta = volumes.MinSeenDelta;
            long maxDelta = volumes.MaxSeenDelta;

            Bar.Deltas.Delta = volumes.BarDelta;
            Bar.Deltas.MinDelta = minDelta;
            Bar.Deltas.MaxDelta = maxDelta;
            Bar.Deltas.DeltaSh = volumes.DeltaSh;
            Bar.Deltas.DeltaSl = volumes.DeltaSl;
            Bar.Deltas.CumulativeDelta = volumes.CumulativeDelta;
            Bar.Deltas.DeltaPercentage = Math.Round(volumes.GetDeltaPercent(), 2);

            Bar.Deltas.MinMaxDeltaRatio = CalculateRatio(Math.Abs(minDelta), Math.Abs(maxDelta));
            Bar.Deltas.MaxMinDeltaRatio = CalculateRatio(Math.Abs(maxDelta), Math.Abs(minDelta));
            Bar.Deltas.DeltaChange = volumes.BarDelta - _baseBar.VolumetricBar.Volumes[_baseBar.CurrentBar - _baseBar.BarsAgo - 1].BarDelta;
        }

        private double CalculateRatio(double numerator, double denominator)
        {
            if (numerator == 0 && denominator == 0)
            {
                return 0;
            }
            if (denominator == 0)
            {
                return Math.Round(numerator, 2);
            }

            return Math.Round(numerator / denominator, 2);
        }
    }
}
