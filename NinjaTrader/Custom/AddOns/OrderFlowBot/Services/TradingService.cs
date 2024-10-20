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
        private readonly TradingState _tradingState;

        public TradingService(EventsContainer eventsContainer)
        {
            _tradingState = new TradingState();

            _eventManager = eventsContainer.EventManager;

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
            _tradingState.ResetTradingState();
        }
    }
}
