using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.States
{
    public class TradingState
    {
        public string TriggeredName { get; set; }
        public bool StrategyTriggered { get; set; }
        public Direction TriggeredDirection { get; set; }
        public Direction SelectedTradeDirection { get; set; }

        public TradingState()
        {
            InitializeTradingState();
            InitializeManualTradingState();
        }

        private void InitializeTradingState()
        {
            TriggeredName = "None";
            StrategyTriggered = false;
            TriggeredDirection = Direction.Flat;
        }

        private void InitializeManualTradingState()
        {
            SelectedTradeDirection = Direction.Any;
        }

        public void ResetTradingState()
        {
            InitializeTradingState();
        }
    }
}
