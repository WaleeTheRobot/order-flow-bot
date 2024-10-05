using NinjaTrader.Cbi;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;

namespace NinjaTrader.NinjaScript.Strategies
{
    public partial class OrderFlowBot
    {
        private int _lastTradeBarNumber;
        private bool _entryLong;
        private bool _entryShort;
        private string _entryName;

        #region Strategies

        private void ResetStrategy()
        {
            UpdateCurrentDataBarAndTradingState();

            Print($"Exit | ${_entryName}");

            _entryLong = false;
            _entryShort = false;
            _entryName = "";

            // Prevent re-entry on previous exit bar
            _lastTradeBarNumber = _currentDataBar.BarNumber + 1;

            //_strategiesController.ResetBackTestingStrategy();
            _eventsContainer.TradingEvents.ResetTradingState();
        }

        private void CloseStrategyPosition()
        {
            if (Position.MarketPosition == MarketPosition.Long)
            {
                ExitLong();
                ResetStrategy();
            }
            else if (Position.MarketPosition == MarketPosition.Short)
            {
                ExitShort();
                ResetStrategy();
            }
        }

        private bool AllowCheckStrategies()
        {
            if (_currentTradingState.SelectedTradeDirection == Direction.Flat || _currentDataBar.BarNumber <= _lastTradeBarNumber)
            {
                return false;
            }

            return true;
        }

        private void CheckStrategies()
        {
            if (!AllowCheckStrategies())
            {
                return;
            }
        }

        #endregion
    }
}
