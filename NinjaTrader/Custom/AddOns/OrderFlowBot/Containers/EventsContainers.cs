using NinjaTrader.Custom.AddOns.OrderFlowBot.Events;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Containers
{
    public class EventsContainer
    {
        public EventManager EventManager { get; set; }
        public DataBarEvents DataBarEvents { get; set; }
        public TradingEvents TradingEvents { get; set; }
        public StrategiesEvents StrategiesEvents { get; set; }
        public TechnicalLevelsEvents TechnicalLevelsEvents { get; set; }

        public EventsContainer()
        {
            EventManager = new EventManager();
            DataBarEvents = new DataBarEvents(EventManager);
            TradingEvents = new TradingEvents(EventManager);
            StrategiesEvents = new StrategiesEvents(EventManager);
            TechnicalLevelsEvents = new TechnicalLevelsEvents(EventManager);
        }
    }
}
