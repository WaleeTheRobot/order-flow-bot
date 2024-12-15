using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.Strategies;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Services;

namespace NinjaTrader.Custom.AddOns.OrderFlowBot.Containers
{
    public class ServicesContainer
    {
        public DataBarService DataBarService { get; set; }
        public StrategiesService StrategiesService { get; set; }
        public TradingService TradingService { get; set; }
        public TechnicalLevelsService TechnicalLevelsService { get; set; }
        public TradeAnalysisService TradeAnalysisService { get; set; }
        public MessagingService MessagingService { get; set; }

        public ServicesContainer(EventsContainer eventsContainer, IBacktestData backtestData)
        {
            DataBarService = new DataBarService(eventsContainer);
            TradingService = new TradingService(eventsContainer, backtestData);
            StrategiesService = new StrategiesService(eventsContainer);
            TechnicalLevelsService = new TechnicalLevelsService(eventsContainer);
            TradeAnalysisService = new TradeAnalysisService(eventsContainer);
            MessagingService = new MessagingService(eventsContainer);
        }
    }
}
