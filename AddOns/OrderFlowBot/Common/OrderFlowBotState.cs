using System.Collections.Generic;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot
{
    public class OrderFlowBotState
    {
        public bool BackTestingEnabled { get; set; }
        public string BackTestingStrategyName { get; set; }
        public bool AutoTradeEnabled { get; set; }
        public Direction SelectedTradeDirection { get; set; }
        public Direction ValidStrategyDirection { get; set; }
        public string ValidStrategy { get; set; }
        public bool DisableTrading { get; set; }
        public List<string> SelectedStrategies { get; set; }
        public double TriggerStrikePrice { get; set; }
        public bool StrikePriceTriggered { get; set; }

        public OrderFlowBotState()
        {
            BackTestingEnabled = false;
            BackTestingStrategyName = "";
            AutoTradeEnabled = false;
            SelectedTradeDirection = Direction.Flat;
            ValidStrategyDirection = Direction.Flat;
            ValidStrategy = "None";
            SelectedStrategies = new List<string>();
            DisableTrading = false;
            TriggerStrikePrice = 0;
            StrikePriceTriggered = false;
        }
    }
}
