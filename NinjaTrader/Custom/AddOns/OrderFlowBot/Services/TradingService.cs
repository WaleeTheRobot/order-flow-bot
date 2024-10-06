using NinjaTrader.Custom.AddOns.OrderFlowBot.Containers;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Events;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.Strategies;
using NinjaTrader.Custom.AddOns.OrderFlowBot.States;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Services
{
    public class TradingService
    {
        private readonly EventManager _eventManager;
        private readonly TradingEvents _tradingEvents;
        private TradingState _tradingState;

        public TradingService(EventsContainer eventsContainer)
        {
            _tradingState = new TradingState();

            _eventManager = eventsContainer.EventManager;

            _tradingEvents = eventsContainer.TradingEvents;
            _tradingEvents.OnGetTradingState += HandleGetTradingState;
            _tradingEvents.OnStrategyTriggered += HandleStrategyTriggered;
            _tradingEvents.OnResetTradingState += HandleResetTradingState;
        }

        private TradingState HandleGetTradingState()
        {
            return _tradingState;
        }

        private void HandleStrategyTriggered(StrategyTriggeredDataProvider strategyTriggeredDataProvider)
        {
            _tradingState.StrategyTriggered = strategyTriggeredDataProvider.StrategyTriggered;
            _tradingState.TriggeredName = strategyTriggeredDataProvider.TriggeredName;
            _tradingState.TriggeredDirection = strategyTriggeredDataProvider.TriggeredDirection;
        }

        private void HandleResetTradingState()
        {
            _tradingState.ResetTradingState();
        }
    }
}
