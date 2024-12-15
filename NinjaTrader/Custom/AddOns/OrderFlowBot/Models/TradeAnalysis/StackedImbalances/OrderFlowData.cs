using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.DataBars;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Models.TradeAnalysis.StackedImbalances
{
    public class OrderFlowData
    {
        public double DeltaPercentage { get; set; }
        public double ImbalanceMetric { get; set; }

        public OrderFlowData()
        {
        }

        public OrderFlowData(IReadOnlyDataBar dataBar)
        {
            DeltaPercentage = dataBar.Deltas.DeltaPercentage;
            ImbalanceMetric = CalculateImbalanceMetric(
                dataBar.Imbalances.AskImbalances.Count,
                dataBar.Imbalances.BidImbalances.Count
            );
        }

        private static double CalculateImbalanceMetric(int askImbalances, int bidImbalances, double tolerance = 1e-10)
        {
            double imbalanceDifference = askImbalances - bidImbalances;
            double totalImbalances = askImbalances + bidImbalances;

            // Preserve magnitude while scaling by total imbalances
            return totalImbalances > tolerance
                ? imbalanceDifference * (totalImbalances / (totalImbalances + tolerance))
                : imbalanceDifference;
        }
    }
}
