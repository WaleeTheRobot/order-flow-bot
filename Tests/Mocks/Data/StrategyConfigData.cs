using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.Strategies;

namespace OrderFlowBot.Tests.Mocks.Data
{
    public class StrategyConfigData : IStrategyData
    {
        public string Name { get; set; }
        public bool StrategyTriggered { get; set; }
        public Direction TriggeredDirection { get; set; }

        public StrategyConfigData()
        {
            Name = "Stacked Imbalances";
            StrategyTriggered = true;
            TriggeredDirection = Direction.Long;
        }

        public void UpdateTriggeredDataProvider(Direction triggeredDirection, bool strategyTriggered)
        {
            // Do nothing
        }
    }
}
