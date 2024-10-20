using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Containers;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Models.Strategies.Implementations
{
    public class StackedImbalances : StrategyBase
    {
        public StackedImbalances(EventsContainer eventsContainer) : base(eventsContainer)
        {
            StrategyData.Name = "Stacked Imbalances";
        }

        public override bool CheckLong()
        {
            return IsBullishBar() && HasValidAskStackedImbalance();
        }

        public override bool CheckShort()
        {
            return IsBearishBar() && HasValidBidStackedImbalance();
        }

        private bool IsBullishBar()
        {
            return currentDataBar.BarType == BarType.Bullish;
        }

        private bool IsBearishBar()
        {
            return currentDataBar.BarType == BarType.Bearish;
        }

        private bool HasValidAskStackedImbalance()
        {
            return currentDataBar.Imbalances.HasAskStackedImbalances;
        }

        private bool HasValidBidStackedImbalance()
        {
            return currentDataBar.Imbalances.HasBidStackedImbalances;
        }
    }
}
