using System.Collections.Generic;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot
{
    public class OrderFlowBotState
    {
        public bool BackTestingEnabled { get; set; }
        public bool AutoTradeEnabled { get; set; }
        public Direction SelectedTradeDirection { get; set; }
        public Direction ValidStrategyDirection { get; set; }
        public OrderFlowBotStrategy ValidStrategy { get; set; }
        public List<OrderFlowBotStrategy> SelectedStrategies { get; set; }

        public OrderFlowBotState()
        {
            BackTestingEnabled = false;
            AutoTradeEnabled = false;
            SelectedTradeDirection = Direction.Flat;
            ValidStrategyDirection = Direction.Flat;
            ValidStrategy = OrderFlowBotStrategy.None;
            SelectedStrategies = new List<OrderFlowBotStrategy>();
        }
    }
}
