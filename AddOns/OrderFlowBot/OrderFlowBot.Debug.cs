using NinjaTrader.NinjaScript.AddOns.OrderFlowBot;

namespace NinjaTrader.NinjaScript.Strategies
{
    // Add to OrderFlowBot directory for debugging or verification
    public partial class OrderFlowBot : Strategy
    {
        public void PrintDataBar(OrderFlowBotDataBar dataBar)
        {

            Print(string.Format("{0}", ToDay(Time[0])));
            Print(string.Format("Time: {0}", dataBar.Time));
            Print(string.Format("Bar Number: {0}", dataBar.BarNumber));
            Print(string.Format("Volume: {0}", dataBar.Volumes.Volume));

            /*
            // Technical Levels Example
            Print("Count: " + _technicalLevels.Pivots.Count);
            Print("Current Looking For High: " + _technicalLevels.IsLookingForHigh);
            Print("Current Pivot: ");
            Print("BarNumber: " + _technicalLevels.CurrentPivot.BarNumber + ", Price: " + _technicalLevels.CurrentPivot.Price + ", DisplayLevel: " + _technicalLevels.CurrentPivot.DisplayLevel + ", IsHigh: " + _technicalLevels.CurrentPivot.IsHigh);
            Print("\n");

            var lastPivots = _technicalLevels.Pivots.Skip(Math.Max(0, _technicalLevels.Pivots.Count - 6)).Take(6).ToList();

            foreach (var pivot in lastPivots)
            {
                Print("BarNumber: " + pivot.BarNumber + ", Price: " + pivot.Price + ", DisplayLevel: " + pivot.DisplayLevel + "IsHigh: " + pivot.IsHigh);
            }

            Print("***************");
            Print("\n");
            */
            /*
            Print(string.Format("Bid Imbalances: {0}", dataBar.Imbalances.BidImbalances.Count));
            Print(string.Format("Ask Imbalances: {0}", dataBar.Imbalances.AskImbalances.Count));
            Print(string.Format("Bid Stacked Imbalances: {0}", dataBar.Imbalances.BidStackedImbalances.Count));
            Print(string.Format("Ask Stacked Imbalances: {0}", dataBar.Imbalances.AskStackedImbalances.Count));

            
            Print(string.Format("Bar Type: {0}", dataBar.BarType));

            Print(string.Format("High: {0}", dataBar.Prices.High));
            Print(string.Format("Low: {0}", dataBar.Prices.Low));
            Print(string.Format("Open: {0}", dataBar.Prices.Open));
            Print(string.Format("Close: {0}", dataBar.Prices.Close));

            

            Print(string.Format("BuyingVolume: {0}", dataBar.Volumes.BuyingVolume));
            Print(string.Format("AskSinglePrint: {0}", dataBar.Volumes.HasAskSinglePrint));
            Print(string.Format("BidSinglePrint: {0}", dataBar.Volumes.HasBidSinglePrint));
            Print(string.Format("SellingVolume: {0}", dataBar.Volumes.SellingVolume));
            Print(string.Format("PointOfControl: {0}", dataBar.Volumes.PointOfControl));

            Print("Bid/Ask Volume Per Bar:");
            foreach (var kvp in dataBar.Volumes.BidAskVolumes)
            {
                Print(string.Format("{0} : {1}", kvp.BidVolume, kvp.AskVolume));
            }

            Print(string.Format("Value Area High: {0}", dataBar.Volumes.ValueAreaHighPrice));
            Print(string.Format("Value Area Low: {0}", dataBar.Volumes.ValueAreaLowPrice));

            Print(string.Format("Bid Ratio: {0}", dataBar.Ratios.BidRatio));
            Print(string.Format("Valid Bid Ratio: {0}", dataBar.Ratios.HasValidBidExhaustionRatio));
            Print(string.Format("Ask Ratio: {0}", dataBar.Ratios.AskRatio));
            Print(string.Format("Valid Ask Ratio: {0}", dataBar.Ratios.HasValidAskExhaustionRatio));
            Print(string.Format("Last Valid Bid Ratio Price: {0}", dataBar.Ratios.LastValidBidRatioPrice));
            Print(string.Format("Last Valid Ask Ratio Price: {0}", dataBar.Ratios.LastValidAskRatioPrice));

            Print(string.Format("Delta: {0}", dataBar.Deltas.Delta));
            Print(string.Format("Min Delta: {0}", dataBar.Deltas.MinDelta));
            Print(string.Format("Max Delta: {0}", dataBar.Deltas.MaxDelta));
            Print(string.Format("Cumulative Delta: {0}", dataBar.Deltas.CumulativeDelta));
            Print(string.Format("Delta Percentage: {0}", dataBar.Deltas.DeltaPercentage));
            Print(string.Format("MinMax Delta %: {0}", dataBar.Deltas.MinMaxDeltaRatio));
            Print(string.Format("MaxMin Delta %: {0}", dataBar.Deltas.MaxMinDeltaRatio));
            Print(string.Format("Delta Change: {0}", dataBar.Deltas.DeltaChange));
            

            Print("Bid Imbalances");
            Print("[");
            foreach (var kvp in dataBar.Imbalances.BidImbalances)
            {
                Print(string.Format("{0} : {1}", kvp.Price, kvp.Volume));
            }
            Print("]");
            Print("Ask Imbalances");
            Print("[");
            foreach (var kvp in dataBar.Imbalances.AskImbalances)
            {
                Print(string.Format("{0} : {1}", kvp.Price, kvp.Volume));
            }
            Print("]");

            Print("Stacked Bid Imbalances");
            Print("[");
            foreach (var kvp in dataBar.Imbalances.BidStackedImbalances)
            {
                Print(string.Format("{0} : {1}", kvp.Price, kvp.Volume));
            }
            Print("]");
            Print("Stacked Ask Imbalances");
            Print("[");
            foreach (var kvp in dataBar.Imbalances.AskStackedImbalances)
            {
                Print(string.Format("{0} : {1}", kvp.Price, kvp.Volume));
            }
            Print("]");

            */

            Print("\n");
        }
    }
}
