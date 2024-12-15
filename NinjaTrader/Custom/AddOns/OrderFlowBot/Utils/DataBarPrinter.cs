using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Events;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.DataBars;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Utils
{
    public static class DataBarPrinter
    {
        private static EventManager _eventManager;

        public static void PrintDataBar(
            EventManager eventManager,
            IReadOnlyDataBar dataBar,
            IDataBarPrintConfig config
        )
        {
            _eventManager = eventManager;

            if (config.ShowBasic)
            {
                PrintBasic(dataBar);
            }

            if (config.ShowDeltas)
            {
                PrintDelta(dataBar);
            }

            if (config.ShowImbalances)
            {
                PrintImbalances(dataBar);
            }

            if (config.ShowPrices)
            {
                PrintPrices(dataBar);
            }

            if (config.ShowRatios)
            {
                PrintRatios(dataBar);
            }

            if (config.ShowVolumes)
            {
                PrintVolumes(dataBar, config);
            }

            if (config.ShowCumulativeDeltaBar)
            {
                PrintCumulativeDeltaBar(dataBar);
            }

            Print("\n");
        }

        private static void PrintBasic(IReadOnlyDataBar dataBar)
        {
            Print("**** Basic ****");
            Print(string.Format("Time: {0}", dataBar.Time));
            Print(string.Format("Day: {0}", dataBar.Day));
            Print(string.Format("Bar Number: {0}", dataBar.BarNumber));
            Print(string.Format("Bar Type: {0}", dataBar.BarType));
            Print(string.Format("Delta: {0}", dataBar.Deltas.Delta));
            Print(string.Format("Volume: {0}", dataBar.Volumes.Volume));
        }

        private static void PrintDelta(IReadOnlyDataBar dataBar)
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

        private static void PrintImbalances(IReadOnlyDataBar dataBar)
        {
            Print("**** Imbalances ****");
            Print("Bid Imbalances:");
            foreach (var kvp in dataBar.Imbalances.BidImbalances)
            {
                Print(string.Format("{0} : {1}", kvp.Price, kvp.Volume));
            }

            Print("Ask Imbalances:");
            foreach (var kvp in dataBar.Imbalances.AskImbalances)
            {
                Print(string.Format("{0} : {1}", kvp.Price, kvp.Volume));
            }

            Print(string.Format("Has Bid Stacked Imbalances {0}", dataBar.Imbalances.HasBidStackedImbalances));
            Print(string.Format("Has Ask Stacked Imbalances {0}", dataBar.Imbalances.HasAskStackedImbalances));

            Print("Stacked Bid Imbalances:");
            foreach (var kvp in dataBar.Imbalances.BidStackedImbalances)
            {
                Print(string.Format("{0} : {1}", kvp.Price, kvp.Volume));
            }

            Print("Stacked Ask Imbalances:");
            foreach (var kvp in dataBar.Imbalances.AskStackedImbalances)
            {
                Print(string.Format("{0} : {1}", kvp.Price, kvp.Volume));
            }
        }

        private static void PrintPrices(IReadOnlyDataBar dataBar)
        {
            Print("**** Prices ****");
            Print(string.Format("High: {0}", dataBar.Prices.High));
            Print(string.Format("Low: {0}", dataBar.Prices.Low));
            Print(string.Format("Open: {0}", dataBar.Prices.Open));
            Print(string.Format("Close: {0}", dataBar.Prices.Close));
        }

        private static void PrintRatios(IReadOnlyDataBar dataBar)
        {
            Print("**** Ratios ****");
            Print(string.Format("Bid Ratio: {0}", dataBar.Ratios.BidRatio));
            Print(string.Format("Ask Ratio: {0}", dataBar.Ratios.AskRatio));
        }

        private static void PrintVolumes(IReadOnlyDataBar dataBar, IDataBarPrintConfig config)
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
                    Print(string.Format("{0} : {1} : {2}", kvp.BidVolume, kvp.AskVolume, kvp.Price));
                }
            }
        }

        private static void PrintCumulativeDeltaBar(IReadOnlyDataBar dataBar)
        {
            Print("**** Cumulative Delta Bar ****");
            Print(string.Format("High: {0}", dataBar.CumulativeDeltaBar.High));
            Print(string.Format("Low: {0}", dataBar.CumulativeDeltaBar.Low));
            Print(string.Format("Open: {0}", dataBar.CumulativeDeltaBar.Open));
            Print(string.Format("Close: {0}", dataBar.CumulativeDeltaBar.Close));
        }

        private static void Print(string message)
        {
            _eventManager.PrintMessage(message);
        }
    }
}
