using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Models.Strategies
{
    public class StrategyTriggeredDataProvider
    {
        public string TriggeredName { get; set; }
        public Direction TriggeredDirection { get; set; }
        public bool StrategyTriggered { get; set; }

        public StrategyTriggeredDataProvider(
            string triggeredName,
            Direction triggeredDirection,
            bool strategyTriggered = true
        )
        {
            TriggeredName = triggeredName;
            TriggeredDirection = triggeredDirection;
            StrategyTriggered = strategyTriggered;
        }
    }
}
