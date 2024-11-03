using NinjaTrader.Custom.AddOns.OrderFlowBot.Containers;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Events;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.Strategies;
using NinjaTrader.Custom.AddOns.OrderFlowBot.States;
using Direction = NinjaTrader.Custom.AddOns.OrderFlowBot.Configs.Direction;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Services
{
    public class TradingService
    {
        private readonly EventManager _eventManager;
        private readonly TradingEvents _tradingEvents;
        private readonly TradingState _tradingState;

        public TradingService(EventsContainer eventsContainer)
        {
            _eventManager = eventsContainer.EventManager;
            _tradingState = new TradingState();

            _tradingEvents = eventsContainer.TradingEvents;
            _tradingEvents.OnGetTradingState += HandleGetTradingState;
            _tradingEvents.OnStrategyTriggered += HandleStrategyTriggered;
            _tradingEvents.OnResetTradingState += HandleResetTradingState;
        }

        private IReadOnlyTradingState HandleGetTradingState()
        {
            return _tradingState;
        }

        private void HandleStrategyTriggered(StrategyData strategyTriggeredData)
        {
            _tradingState.SetTriggeredTradingState(
                strategyTriggeredData.Name,
                strategyTriggeredData.StrategyTriggered,
                strategyTriggeredData.TriggeredDirection
            );

            _tradingEvents.StrategyTriggeredProcessed();
        }

        private void HandleResetTradingState()
        {
            _tradingState.SetInitialTriggeredState();
        }

        #region User Interface

        public void UpdateIsTradingEnabled(bool isEnabled)
        {
            _tradingState.IsTradingEnabled = isEnabled;

            _eventManager.PrintMessage($"IsTradingEnabled: {_tradingState.IsTradingEnabled}");
        }

        public void UpdateIsAutoTradeEnabled(bool isEnabled)
        {
            _tradingState.IsAutoTradeEnabled = isEnabled;

            _eventManager.PrintMessage($"IsAutoTradeEnabled: {_tradingState.IsAutoTradeEnabled}");
        }

        public void UpdateIsAlertEnabled(bool isEnabled)
        {
            _tradingState.IsAlertEnabled = isEnabled;

            _eventManager.PrintMessage($"IsAlertEnabled: {_tradingState.IsAlertEnabled}");
        }

        public void HandleResetDirectionTriggered()
        {
            _tradingState.SetInitialTradeDirection();
            _eventManager.PrintMessage($"ResetDirectionTriggered: {_tradingState.TriggerStrikePrice}");
            _eventManager.PrintMessage($"ResetDirectionTriggered: {_tradingState.StandardInverse}");
            _eventManager.PrintMessage($"ResetDirectionTriggered: {_tradingState.SelectedTradeDirection}");
        }

        public void UpdateSelectedTradeDirection(Direction direction)
        {
            _tradingState.SelectedTradeDirection = direction;

            _eventManager.PrintMessage($"SelectedTradeDirection: {_tradingState.SelectedTradeDirection}");
        }

        public void UpdateStandardInverse(Direction direction)
        {
            if (direction == Direction.Standard || direction == Direction.Inverse)
            {
                _tradingState.StandardInverse = direction;
            }
            else
            {
                _tradingState.StandardInverse = Direction.Standard;
            }

            _eventManager.PrintMessage($"StandardInverse: {_tradingState.StandardInverse}");
        }

        public void HandleCloseTriggered()
        {
            _tradingEvents.CloseTriggered();
        }

        public void HandleTriggerStrikePriceTriggered(double price)
        {
            _tradingState.TriggerStrikePrice = price;

            _eventManager.PrintMessage($"TriggerStrikePrice: {_tradingState.TriggerStrikePrice}");
        }

        #endregion
    }
}
