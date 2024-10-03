using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Containers;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Models.Strategies.Implementations
{
    public class StackedImbalances : StrategyBase
    {
        public override string Name { get; set; }
        public override bool StrategyTriggered { get; set; }

        public StackedImbalances(EventsContainer eventsContainer) : base(eventsContainer)
        {
            Name = "Stacked Imbalances";
        }

        public override void CheckLong()
        {
            if (IsBullishBar() && HasValidAskStackedImbalance())
            {
                eventsContainer.TradingEvents.StrategyTriggered(new StrategyTriggeredDataProvider(
                    Name,
                    Direction.Long
                ));
            }
        }

        public override void CheckShort()
        {
            if (IsBearishBar() && HasValidBidStackedImbalance())
            {
                eventsContainer.TradingEvents.StrategyTriggered(new StrategyTriggeredDataProvider(
                    Name,
                    Direction.Short
                ));
            }
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
