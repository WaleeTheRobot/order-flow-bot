using NinjaTrader.Custom.AddOns.OrderFlowBot.Services;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Containers
{
    public class ServicesContainer
    {
        public DataBarService DataBarService { get; private set; }
        public StrategiesService StrategiesService { get; private set; }
        public TradingService TradingService { get; private set; }

        public ServicesContainer(EventsContainer eventsContainer)
        {
            DataBarService = new DataBarService(eventsContainer);
            StrategiesService = new StrategiesService(eventsContainer);
            TradingService = new TradingService(eventsContainer);
        }
    }
}
