using NinjaTrader.Custom.AddOns.OrderFlowBot.Events;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Containers
{
    public class EventsContainer
    {
        public EventManager EventManager { get; private set; }
        public DataBarEvents DataBarEvents { get; private set; }
        public TradingEvents TradingEvents { get; private set; }

        public EventsContainer()
        {
            EventManager = new EventManager();
            DataBarEvents = new DataBarEvents(EventManager);
            TradingEvents = new TradingEvents(EventManager);
        }
    }
}
