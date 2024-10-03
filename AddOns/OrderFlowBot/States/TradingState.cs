using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.States
{
    public class TradingState
    {
        public string ValidStrategyName { get; set; }
        public Direction SelectedTradeDirection { get; set; }
        public Direction ValidStrategyDirection { get; set; }

        public TradingState()
        {
            ValidStrategyName = "None";
            SelectedTradeDirection = Direction.Flat;
            ValidStrategyDirection = Direction.Flat;
        }
    }
}
