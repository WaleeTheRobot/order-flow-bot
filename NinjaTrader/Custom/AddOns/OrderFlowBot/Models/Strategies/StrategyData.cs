using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Models.Strategies
{
    public class StrategyData : IStrategyData
    {
        public string Name { get; set; }
        public Direction TriggeredDirection { get; set; }
        public bool StrategyTriggered { get; set; }

        public StrategyData()
        {
        }

        public StrategyData(
            string name,
            Direction triggeredDirection,
            bool strategyTriggered = false
        )
        {
            Name = name;
            TriggeredDirection = triggeredDirection;
            StrategyTriggered = strategyTriggered;
        }

        public void UpdateTriggeredDataProvider(
            Direction triggeredDirection,
            bool strategyTriggered
        )
        {
            TriggeredDirection = triggeredDirection;
            StrategyTriggered = strategyTriggered;
        }
    }
}
