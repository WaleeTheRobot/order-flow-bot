using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;

namespace OrderFlowBot.Tests.Mocks.Data
{
    public class TradingStateData
    {
        public string TriggeredName { get; private set; }
        public bool StrategyTriggered { get; private set; }
        public Direction TriggeredDirection { get; private set; }
        public Direction SelectedTradeDirection { get; set; }
        public Direction StandardInverse { get; set; }
        public bool IsTradingEnabled { get; set; }
        public bool IsAutoTradeEnabled { get; set; }
        public bool IsAlertEnabled { get; set; }
        public double TriggerStrikePrice { get; set; }

        public TradingStateData()
        {
            TriggeredName = "Stacked Imbalances";
            StrategyTriggered = true;
            TriggeredDirection = Direction.Long;
            SelectedTradeDirection = Direction.Any;
            StandardInverse = Direction.Standard;
            IsTradingEnabled = false;
            IsAutoTradeEnabled = false;
            IsAlertEnabled = false;
            TriggerStrikePrice = 0;
        }
    }
}
