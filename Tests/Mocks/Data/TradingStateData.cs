using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;

namespace OrderFlowBot.Tests.Mocks.Data
{
    public class TradingStateData
    {
        public string TriggeredName { get; set; }
        public bool StrategyTriggered { get; set; }
        public Direction TriggeredDirection { get; set; }
        public Direction SelectedTradeDirection { get; set; }

        public TradingStateData()
        {
            TriggeredName = "Stacked Imbalances";
            StrategyTriggered = true;
            TriggeredDirection = Direction.Long;
            SelectedTradeDirection = Direction.Any;
        }
    }
}
