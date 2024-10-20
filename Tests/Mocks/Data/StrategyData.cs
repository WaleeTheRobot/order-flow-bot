using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;

namespace OrderFlowBot.Tests.Mocks.Data
{
    public class StrategyData
    {
        public string Name { get; set; }
        public bool StrategyTriggered { get; set; }
        public Direction TriggeredDirection { get; set; }

        public StrategyData()
        {
            Name = "Stacked Imbalances";
            StrategyTriggered = true;
            TriggeredDirection = Direction.Long;
        }
    }
}
