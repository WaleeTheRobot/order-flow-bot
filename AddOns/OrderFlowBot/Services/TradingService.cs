using NinjaTrader.Custom.AddOns.OrderFlowBot.Containers;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Events;
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
        }

        private TradingState HandleGetTradingState()
        {
            return _tradingState;
        }
    }
}
