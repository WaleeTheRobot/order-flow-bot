using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Events;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.DataBar;
using System.Collections.Generic;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Services
{
    public class DataBarService
    {
        private readonly EventManager _eventManager;
        private readonly DataBarEvents _dataBarEvents;
        private List<DataBar> _dataBars;
        private DataBar _currentDataBar;

        public DataBarService(EventManager eventManager, DataBarEvents dataBarEvents)
        {
            _eventManager = eventManager;

            _dataBarEvents = dataBarEvents;
            _dataBarEvents.OnUpdateCurrentDataBar += HandleUpdateCurrentDataBar;
            _dataBarEvents.OnUpdateCurrentDataBarList += HandleUpdateCurrentDataBarList;
            _dataBarEvents.OnPrintDataBar += HandlePrintDataBar;

            _dataBars = new List<DataBar>();
            _currentDataBar = new DataBar();
        }

        private void HandleUpdateCurrentDataBar(DataBarDataProvider dataBarDataProvider)
        {
        }

        private void HandleUpdateCurrentDataBarList()
        {
        }

        private void HandlePrintDataBar(DataBarPrintConfig dataBarPrintConfig)
        {
            DataBar dataBar;
            int barsAgo = dataBarPrintConfig.BarsAgo;

            if (barsAgo == 0)
            {
                dataBar = _currentDataBar;
            }
            else
            {
                dataBar = _dataBars[_dataBars.Count - barsAgo];
            }

            if (dataBarPrintConfig.ShowBasic)
            {
                Print("**** Basic ****");
                Print(string.Format("Time: {0}", dataBar.Time));
                Print(string.Format("Bar Number: {0}", dataBar.BarNumber));
                Print(string.Format("Bar Type: {0}", dataBar.BarType));
                Print(string.Format("Delta: {0}", dataBar.Deltas.Delta));
                Print(string.Format("Volume: {0}", dataBar.Volumes.Volume));
            }
            else if (dataBarPrintConfig.ShowDeltas)
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
            else if (dataBarPrintConfig.ShowImbalances)
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
            else if (dataBarPrintConfig.ShowPrices)
            {
                Print("**** Prices ****");
                Print(string.Format("High: {0}", dataBar.Prices.High));
                Print(string.Format("Low: {0}", dataBar.Prices.Low));
                Print(string.Format("Open: {0}", dataBar.Prices.Open));
                Print(string.Format("Close: {0}", dataBar.Prices.Close));
            }
            else if (dataBarPrintConfig.ShowRatios)
            {
                Print("**** Ratios ****");
                Print(string.Format("Bid Ratio: {0}", dataBar.Ratios.BidRatio));
                Print(string.Format("Ask Ratio: {0}", dataBar.Ratios.AskRatio));
            }
            else if (dataBarPrintConfig.ShowVolumes)
            {
                Print("**** Volume ****");
                Print(string.Format("Volume: {0}", dataBar.Volumes.Volume));
                Print(string.Format("Buying Volume: {0}", dataBar.Volumes.BuyingVolume));
                Print(string.Format("Selling Volume: {0}", dataBar.Volumes.SellingVolume));
                Print(string.Format("Point Of Control: {0}", dataBar.Volumes.PointOfControl));
                Print(string.Format("Value Area High: {0}", dataBar.Volumes.ValueAreaHighPrice));
                Print(string.Format("Value Area Low: {0}", dataBar.Volumes.ValueAreaLowPrice));

                if (dataBarPrintConfig.ShowBidAskVolumePerBar)
                {
                    Print("Bid/Ask Volume Per Bar:");
                    foreach (var kvp in dataBar.Volumes.BidAskVolumes)
                    {
                        Print(string.Format("{0} : {1}", kvp.BidVolume, kvp.AskVolume));
                    }
                }
            }

            Print("\n");
        }

        private void Print(string message)
        {
            _eventManager.PrintMessage(message);
        }
    }
}
