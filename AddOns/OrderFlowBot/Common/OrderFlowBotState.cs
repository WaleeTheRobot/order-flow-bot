using System.Collections.Generic;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot
{
    public class OrderFlowBotState
    {
        public bool BackTestingEnabled { get; set; }
        public Direction SelectedTradeDirection { get; set; }
        public Direction ValidStrategyDirection { get; set; }
        public string ValidStrategy { get; set; }
        public MarketDirection MarketDirection { get; set; }
        public List<string> SelectedStrategies { get; set; }

        public OrderFlowBotState()
        {
            BackTestingEnabled = false;
            SelectedTradeDirection = Direction.Flat;
            ValidStrategyDirection = Direction.Flat;
            ValidStrategy = "None";
            SelectedStrategies = new List<string>();
            MarketDirection = MarketDirection.Trend;
        }
    }
}
