using NinjaTrader.Custom.AddOns.OrderFlowBot.DataBar.Dependencies;
using NinjaTrader.NinjaScript.AddOns.OrderFlowBot;
using System;
using System.Collections.Generic;

namespace NinjaTrader.NinjaScript.Strategies
{
    public partial class OrderFlowBot : Strategy
    {
        private OrderFlowBotDataBar GetDataBar(List<OrderFlowBotDataBar> dataBars, int barsAgo)
        {
            OrderFlowBotDataBar dataBar = new OrderFlowBotDataBar();
            NinjaTrader.NinjaScript.BarsTypes.VolumetricBarsType volumetricBar = Bars.BarsSeries.BarsType as NinjaTrader.NinjaScript.BarsTypes.VolumetricBarsType;
            if (volumetricBar == null) return dataBar;

            PopulateBasic(dataBar, barsAgo);
            PopulatePrices(dataBar, barsAgo);
            PopulateVolumeAndImbalances(dataBars, dataBar, volumetricBar, barsAgo);
            PopulateDeltas(dataBar, volumetricBar, barsAgo);

            return dataBar;
        }

        private void PopulateBasic(OrderFlowBotDataBar dataBar, int barsAgo)
        {
            dataBar.Time = ToTime(Time[barsAgo]);
            dataBar.BarNumber = CurrentBar - barsAgo;
        }

        private void PopulatePrices(OrderFlowBotDataBar dataBar, int barsAgo)
        {
            dataBar.Prices.High = High[barsAgo];
            dataBar.Prices.Low = Low[barsAgo];
            dataBar.Prices.Open = Open[barsAgo];
            dataBar.Prices.Close = Close[barsAgo];
            dataBar.SetBarType();
        }

        private void PopulateVolumeAndImbalances(List<OrderFlowBotDataBar> dataBars, OrderFlowBotDataBar dataBar, NinjaTrader.NinjaScript.BarsTypes.VolumetricBarsType volumetricBar, int barsAgo)
        {
            double high = dataBar.Prices.High;
            double low = dataBar.Prices.Low;

            dataBar.Volumes.Volume = volumetricBar.Volumes[CurrentBar - barsAgo].TotalVolume;
            dataBar.Volumes.BuyingVolume = volumetricBar.Volumes[CurrentBar - barsAgo].TotalBuyingVolume;
            dataBar.Volumes.SellingVolume = volumetricBar.Volumes[CurrentBar - barsAgo].TotalSellingVolume;

            double pointOfControl;
            volumetricBar.Volumes[CurrentBar - barsAgo].GetMaximumVolume(null, out pointOfControl);
            dataBar.Volumes.PointOfControl = pointOfControl;

            // Get bid/ask volume for each price in bar
            List<BidAskVolume> bidAskVolumeList = new List<BidAskVolume>();

            while (high >= low)
            {
                BidAskVolume bidAskVolume = new BidAskVolume
                {
                    Price = high,
                    BidVolume = volumetricBar.Volumes[CurrentBar - barsAgo].GetBidVolumeForPrice(high),
                    AskVolume = volumetricBar.Volumes[CurrentBar - barsAgo].GetAskVolumeForPrice(high)
                };

                bidAskVolumeList.Add(bidAskVolume);

                high -= TickSize;
            }

            dataBar.Volumes.BidAskVolumes = bidAskVolumeList;
            dataBar.Volumes.SetVolumeSequencing(bidAskVolumeList, dataBar.BarType, dataBar.Volumes.Volume);
            dataBar.Volumes.SetSinglePrints();
            dataBar.Volumes.SetBidAskPriceVolumeAndVolumeDelta();
            dataBar.Imbalances.SetImbalances(bidAskVolumeList, dataBar.Volumes.ValidBidAskVolumes());
            dataBar.Ratios.SetLastRatioPrices(dataBars);
            dataBar.Ratios.SetRatios(bidAskVolumeList, dataBar.Volumes.ValidBidAskVolumes(), dataBar.BarType);
        }

        private void PopulateDeltas(OrderFlowBotDataBar dataBar, NinjaTrader.NinjaScript.BarsTypes.VolumetricBarsType volumetricBar, int barsAgo)
        {
            long minDelta = volumetricBar.Volumes[CurrentBar - barsAgo].MinSeenDelta;
            long maxDelta = volumetricBar.Volumes[CurrentBar - barsAgo].MaxSeenDelta;

            dataBar.Deltas.Delta = volumetricBar.Volumes[CurrentBar - barsAgo].BarDelta;
            dataBar.Deltas.MinDelta = minDelta;
            dataBar.Deltas.MaxDelta = maxDelta;
            dataBar.Deltas.CumulativeDelta = volumetricBar.Volumes[CurrentBar - barsAgo].CumulativeDelta;
            dataBar.Deltas.DeltaPercentage = Math.Round(volumetricBar.Volumes[CurrentBar - barsAgo].GetDeltaPercent(), 2);

            dataBar.Deltas.MinMaxDeltaRatio = CalculateRatio(Math.Abs(minDelta), Math.Abs(maxDelta));
            dataBar.Deltas.MaxMinDeltaRatio = CalculateRatio(Math.Abs(maxDelta), Math.Abs(minDelta));
            dataBar.Deltas.DeltaChange = volumetricBar.Volumes[CurrentBar - barsAgo].BarDelta - volumetricBar.Volumes[CurrentBar - barsAgo - 1].BarDelta;
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
