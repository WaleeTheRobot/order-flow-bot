using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.States
{
    public class TradingState : IReadOnlyTradingState
    {
        public string TriggeredName { get; private set; }
        public bool StrategyTriggered { get; private set; }
        public Direction TriggeredDirection { get; private set; }
        public Direction SelectedTradeDirection { get; private set; }

        public TradingState()
        {
            InitializeTradingState();
        }

        private void InitializeTradingState()
        {
            TriggeredName = "None";
            StrategyTriggered = false;
            TriggeredDirection = Direction.Flat;
            SelectedTradeDirection = Direction.Any;
        }

        public void SetTriggeredTradingState(
            string name,
            bool strategyTriggered,
            Direction triggeredDirection
        )
        {
            TriggeredName = name;
            StrategyTriggered = strategyTriggered;
            TriggeredDirection = triggeredDirection;
        }

        public void ResetTradingState()
        {
            InitializeTradingState();
        }
    }
}
