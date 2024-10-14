using NinjaTrader.Custom.AddOns.OrderFlowBot.Services;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Containers
{
    public class ServicesContainer
    {
        public DataBarService DataBarService { get; set; }
        public StrategiesService StrategiesService { get; set; }
        public TradingService TradingService { get; set; }

        public ServicesContainer(EventsContainer eventsContainer)
        {
            DataBarService = new DataBarService(eventsContainer);
            StrategiesService = new StrategiesService(eventsContainer);
            TradingService = new TradingService(eventsContainer);
        }
    }
}
