using NinjaTrader.Cbi;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;
using System;

namespace NinjaTrader.NinjaScript.Strategies
{
    // Manages strategies that requires NinjaScript namespace
    public partial class OrderFlowBot : Strategy
    {
        private string _triggeredName;

        public void InitializeStrategyManager()
        {
            _tradingEvents.OnStrategyTriggeredProcessed += HandleStrategyTriggeredProcessed;
            _tradingEvents.OnCloseTriggered += HandleCloseTriggered;

            _triggeredName = "";
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
                ResetBackTestStrategy();
                _eventsContainer.StrategiesEvents.ResetStrategyData();
            }
        }

        private void HandleStrategyTriggeredProcessed()
        {
            _currentDataBar = _dataBarEvents.GetCurrentDataBar();
            _currentTradingState = _tradingEvents.GetTradingState();
            _triggeredName = _currentTradingState.TriggeredName;

            ProcessTriggeredStrategy();
        }

        private void HandleCloseTriggered()
        {
            // TODO: Close position
            Print("Position Closed");
        }

        private void ProcessTriggeredStrategy()
        {
            if (BackTestEnabled)
            {
                ProcessBackTestTriggeredStrategy();
            }
            // else use atm
        }

        private void ResetBackTestStrategy()
        {
            _eventManager.PrintMessage($"Exit | {_currentDataBar.Time} {_triggeredName}", true);

            // Prevent re-entry on exit bar
            _tradingEvents.LastTradedBarNumberTriggered(_dataBarEvents.GetCurrentDataBar().BarNumber);
            _tradingEvents.ResetTradingState();
        }

        #region BackTest

        private void ProcessBackTestTriggeredStrategy()
        {
            if (Position.MarketPosition == MarketPosition.Flat)
            {
                if (_currentTradingState.TriggeredDirection == Direction.Long)
                {
                    SetProfitTarget(_triggeredName, CalculationMode.Ticks, Target);
                    SetStopLoss(_triggeredName, CalculationMode.Ticks, Stop, false);
                    EnterLong(Quantity, _triggeredName);

                    _tradingEvents.LastTradedBarNumberTriggered(_currentDataBar.BarNumber);
                    _eventManager.PrintMessage($"Enter Long | {_currentDataBar.Time} {_triggeredName}");

                    return;
                }

                if (_currentTradingState.TriggeredDirection == Direction.Short)
                {
                    SetProfitTarget(_triggeredName, CalculationMode.Ticks, Target);
                    SetStopLoss(_triggeredName, CalculationMode.Ticks, Stop, false);
                    EnterShort(Quantity, _triggeredName);

                    _tradingEvents.LastTradedBarNumberTriggered(_currentDataBar.BarNumber);
                    _eventManager.PrintMessage($"Enter Short | {_currentDataBar.Time} {_triggeredName}");
                }
            }
        }

        #endregion
    }
}
