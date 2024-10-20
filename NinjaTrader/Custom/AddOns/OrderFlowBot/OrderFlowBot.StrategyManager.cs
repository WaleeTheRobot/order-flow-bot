using NinjaTrader.Cbi;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;
using System;

namespace NinjaTrader.NinjaScript.Strategies
{
    public partial class OrderFlowBot : Strategy
    {

        private string _triggeredName;
        private int _lastTradeBarNumber;

        public void InitializeStrategyManager()
        {
            _tradingEvents.OnStrategyTriggeredProcessed += HandleStrategyTriggeredProcessed;

            _triggeredName = "";
            _lastTradeBarNumber = 0;
        }

        protected override void OnExecutionUpdate(
            Execution execution,
            string executionId,
            double price,
            int quantity,
            MarketPosition marketPosition,
            string orderId,
            DateTime time)
        {
            if (Position.MarketPosition == MarketPosition.Flat)
            {
                ResetStrategy();
            }
        }

        private void HandleStrategyTriggeredProcessed()
        {
            _currentDataBar = _dataBarEvents.GetCurrentDataBar();
            _currentTradingState = _tradingEvents.GetTradingState();
            _triggeredName = _currentTradingState.TriggeredName;

            ProcessTriggeredStrategy();
        }

        private void ProcessTriggeredStrategy()
        {
            if (AllowStrategyEntry())
            {
                if (_currentTradingState.TriggeredDirection == Direction.Long)
                {
                    SetProfitTarget(_triggeredName, CalculationMode.Ticks, Target);
                    SetStopLoss(_triggeredName, CalculationMode.Ticks, Stop, false);
                    EnterLong(Quantity, _triggeredName);

                    _lastTradeBarNumber = _currentDataBar.BarNumber;
                    _eventManager.PrintMessage($"Enter Long | {_triggeredName}");

                    return;
                }

                if (_currentTradingState.TriggeredDirection == Direction.Short)
                {
                    SetProfitTarget(_triggeredName, CalculationMode.Ticks, Target);
                    SetStopLoss(_triggeredName, CalculationMode.Ticks, Stop, false);
                    EnterShort(Quantity, _triggeredName);

                    _lastTradeBarNumber = _currentDataBar.BarNumber;
                    _eventManager.PrintMessage($"Enter Short | {_triggeredName}");
                }
            }
        }

        private bool AllowStrategyEntry()
        {
            if (
                Position.MarketPosition == MarketPosition.Flat &&
                _currentDataBar.BarNumber <= _lastTradeBarNumber
            )
            {
                // Reset in case strategy was triggered within same _lastTradeBarNumber
                _tradingEvents.ResetTradingState();

                return false;
            }

            return true;
        }

        private void ResetStrategy()
        {
            _eventManager.PrintMessage($"Exit | {_triggeredName}", true);

            // Prevent re-entry on exit bar
            _lastTradeBarNumber = _dataBarEvents.GetCurrentDataBar().BarNumber;
            _tradingEvents.ResetTradingState();
        }
    }
}
