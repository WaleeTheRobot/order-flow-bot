﻿using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Events;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.DataBar;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Utils
{
    public static class DataBarPrinter
    {
        private static EventManager _eventManager;

        public static void PrintDataBar(EventManager eventManager, DataBar dataBar, DataBarPrintConfig dataBarPrintConfig)
        {
            _eventManager = eventManager;

            if (dataBarPrintConfig.ShowBasic)
            {
                PrintBasic(dataBar);
            }

            if (dataBarPrintConfig.ShowDeltas)
            {
                PrintDelta(dataBar);
            }

            if (dataBarPrintConfig.ShowImbalances)
            {
                PrintImbalances(dataBar);
            }

            if (dataBarPrintConfig.ShowPrices)
            {
                PrintPrices(dataBar);
            }

            if (dataBarPrintConfig.ShowRatios)
            {
                PrintRatios(dataBar);
            }

            if (dataBarPrintConfig.ShowVolumes)
            {
                PrintVolumes(dataBar, dataBarPrintConfig);
            }

            Print("\n");
        }

        private static void PrintBasic(DataBar dataBar)
        {
            Print("**** Basic ****");
            Print(string.Format("Time: {0}", dataBar.Time));
            Print(string.Format("Bar Number: {0}", dataBar.BarNumber));
            Print(string.Format("Bar Type: {0}", dataBar.BarType));
            Print(string.Format("Delta: {0}", dataBar.Deltas.Delta));
            Print(string.Format("Volume: {0}", dataBar.Volumes.Volume));
        }

        private static void PrintDelta(DataBar dataBar)
        {
            Print("**** Deltas ****");
            Print(string.Format("Delta: {0}", dataBar.Deltas.Delta));
            Print(string.Format("Min Delta: {0}", dataBar.Deltas.MinDelta));
            Print(string.Format("Max Delta: {0}", dataBar.Deltas.MaxDelta));
            Print(string.Format("Cumulative Delta: {0}", dataBar.Deltas.CumulativeDelta));
            Print(string.Format("Delta Percentage: {0}", dataBar.Deltas.DeltaPercentage));
            Print(string.Format("MinMax Delta %: {0}", dataBar.Deltas.MinMaxDeltaRatio));
            Print(string.Format("MaxMin Delta %: {0}", dataBar.Deltas.MaxMinDeltaRatio));
            Print(string.Format("Delta Change: {0}", dataBar.Deltas.DeltaChange));
            Print(string.Format("Delta Sl: {0}", dataBar.Deltas.DeltaSl));
            Print(string.Format("Delta Sh: {0}", dataBar.Deltas.DeltaSh));
        }

        private static void PrintImbalances(DataBar dataBar)
        {
            Print("**** Imbalances ****");
            Print("Bid Imbalances");
            foreach (var kvp in dataBar.Imbalances.BidImbalances)
            {
                Print(string.Format("{0} : {1}", kvp.Price, kvp.Volume));
            }

            Print("Ask Imbalances");
            foreach (var kvp in dataBar.Imbalances.AskImbalances)
            {
                Print(string.Format("{0} : {1}", kvp.Price, kvp.Volume));
            }

            Print(string.Format("Has Bid Stacked Imbalances {0}", dataBar.Imbalances.HasBidStackedImbalances));
            Print(string.Format("Has Ask Stacked Imbalances {0}", dataBar.Imbalances.HasAskStackedImbalances));

            Print("Stacked Bid Imbalances");
            foreach (var kvp in dataBar.Imbalances.BidStackedImbalances)
            {
                Print(string.Format("{0} : {1}", kvp.Price, kvp.Volume));
            }

            Print("Stacked Ask Imbalances");
            foreach (var kvp in dataBar.Imbalances.AskStackedImbalances)
            {
                Print(string.Format("{0} : {1}", kvp.Price, kvp.Volume));
            }
        }

        private static void PrintPrices(DataBar dataBar)
        {
            Print("**** Prices ****");
            Print(string.Format("High: {0}", dataBar.Prices.High));
            Print(string.Format("Low: {0}", dataBar.Prices.Low));
            Print(string.Format("Open: {0}", dataBar.Prices.Open));
            Print(string.Format("Close: {0}", dataBar.Prices.Close));
        }

        private static void PrintRatios(DataBar dataBar)
        {
            Print("**** Ratios ****");
            Print(string.Format("Bid Ratio: {0}", dataBar.Ratios.BidRatio));
            Print(string.Format("Ask Ratio: {0}", dataBar.Ratios.AskRatio));
        }

        private static void PrintVolumes(DataBar dataBar, DataBarPrintConfig config)
        {
            Print("**** Volume ****");
            Print(string.Format("Volume: {0}", dataBar.Volumes.Volume));
            Print(string.Format("Buying Volume: {0}", dataBar.Volumes.BuyingVolume));
            Print(string.Format("Selling Volume: {0}", dataBar.Volumes.SellingVolume));
            Print(string.Format("Point Of Control: {0}", dataBar.Volumes.PointOfControl));
            Print(string.Format("Value Area High: {0}", dataBar.Volumes.ValueAreaHighPrice));
            Print(string.Format("Value Area Low: {0}", dataBar.Volumes.ValueAreaLowPrice));

            if (config.ShowBidAskVolumePerBar)
            {
                Print("Bid/Ask Volume Per Bar:");
                foreach (var kvp in dataBar.Volumes.BidAskVolumes)
                {
                    Print(string.Format("{0} : {1}", kvp.BidVolume, kvp.AskVolume));
                }
            }
        }

        private static void Print(string message)
        {
            _eventManager.PrintMessage(message);
        }
    }
}
