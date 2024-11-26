using NinjaTrader.Custom.AddOns.OrderFlowBot.Containers;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Events;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.Strategies;
using NinjaTrader.Custom.AddOns.OrderFlowBot.States;
using Direction = NinjaTrader.Custom.AddOns.OrderFlowBot.Configs.Direction;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Services
{
    // Handles the TradingState
    public class TradingService
    {
        private readonly EventManager _eventManager;
        private readonly TradingEvents _tradingEvents;
        private readonly TradingState _tradingState;

        public TradingService(EventsContainer eventsContainer, IBacktestData backtestData)
        {
            _eventManager = eventsContainer.EventManager;
            _tradingState = new TradingState(backtestData);

            _tradingEvents = eventsContainer.TradingEvents;
            _tradingEvents.OnGetTradingState += HandleGetTradingState;
            _tradingEvents.OnStrategyTriggered += HandleStrategyTriggered;
            _tradingEvents.OnResetTriggeredTradingState += HandleResetTriggeredTradingState;
            _tradingEvents.OnLastTradedBarNumberTriggered += HandleLastTradedBarNumberTriggered;
            _tradingEvents.OnMarketPositionTriggered += HandleMarketPositionTriggered;
            _tradingEvents.OnCurrentBarNumberTriggered += HandleCurrentBarNumberTriggered;
            _tradingEvents.OnResetTriggerStrikePrice += HandleResetTriggerStrikePrice;
            _tradingEvents.OnResetSelectedTradeDirection += HandleResetSelectedTradeDirection;
        }

        private IReadOnlyTradingState HandleGetTradingState()
        {
            return _tradingState;
        }

        private void HandleStrategyTriggered(IStrategyData strategyTriggeredData)
        {
            _tradingState.SetTriggeredTradingState(
                strategyTriggeredData.Name,
                strategyTriggeredData.StrategyTriggered,
                strategyTriggeredData.TriggeredDirection
            );

            _tradingEvents.StrategyTriggeredProcessed();
        }

        private void HandleResetTriggeredTradingState()
        {
            _tradingState.SetInitialTriggeredState();
        }

        private void HandleLastTradedBarNumberTriggered(int barNumber)
        {
            _tradingState.LastTradedBarNumber = barNumber;
        }

        private void HandleMarketPositionTriggered(bool hasMarketPosition)
        {
            _tradingState.HasMarketPosition = hasMarketPosition;
        }

        private void HandleCurrentBarNumberTriggered(int barNumber)
        {
            _tradingState.CurrentBarNumber = barNumber;
        }

        private void HandleResetTriggerStrikePrice()
        {
            _tradingState.ResetTriggerStrikePrice();
        }

        private void HandleResetSelectedTradeDirection()
        {
            _tradingState.ResetSelectedTradeDirection();
        }

        #region User Interface

        public void HandleEnabledDisabledTriggered(bool isEnabled)
        {
            _tradingState.IsTradingEnabled = isEnabled;
            _tradingState.SetInitialTriggeredState();
            HandleCloseTriggered();
        }

        public void UpdateIsAutoTradeEnabled(bool isEnabled)
        {
            _tradingState.IsAutoTradeEnabled = isEnabled;
        }

        public void UpdateIsAlertEnabled(bool isEnabled)
        {
            _tradingState.IsAlertEnabled = isEnabled;
        }

        public void HandleResetDirectionTriggered()
        {
            _tradingState.SetInitialTradeDirection();
        }

        public void HandleResetStrategiesTriggered()
        {
            _tradingState.RemoveAllSelectedStrategies();
        }

        public void UpdateSelectedTradeDirection(Direction direction)
        {
            _tradingState.SelectedTradeDirection = direction;
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
        }

        public void HandleCloseTriggered()
        {
            _tradingEvents.CloseTriggered();
        }

        public void HandleTriggerStrikePriceTriggered(double price)
        {
            _tradingState.TriggerStrikePrice = price;
        }

        public void HandleAddSelectedStrategyTriggered(string name)
        {
            _tradingState.AddStrategyByName(name);
        }

        public void HandleRemoveSelectedStrategyTriggered(string name)
        {
            _tradingState.RemoveStrategyByName(name);
        }

        #endregion
    }
}
