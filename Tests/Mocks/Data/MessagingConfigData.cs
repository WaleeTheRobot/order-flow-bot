using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.Strategies;

namespace OrderFlowBot.Tests.Mocks.Data
{
    public class MessagingConfigData
    {
        public EnvironmentType MarketEnvironment { get; set; }
        public string ExternalAnalysisService { get; set; }
        public bool ExternalAnalysisServiceEnabled { get; set; }

        public MessagingConfigData()
        {
            MarketEnvironment = EnvironmentType.Test;
            ExternalAnalysisService = "http://localhost:5000/analyze";
            ExternalAnalysisServiceEnabled = false;
        }
    }
}
