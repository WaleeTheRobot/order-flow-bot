using Moq;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Models.Strategies;
using OrderFlowBot.Tests.Mocks.Data;

namespace OrderFlowBot.Tests.Mocks
{
    public static class StrategyDataMock
    {
        public static Mock<IStrategyData> CreateStrategyConfig()
        {
            var config = new StrategyConfigData();

            var mock = new Mock<IStrategyData>();
            mock.SetupProperty(x => x.Name, config.Name);
            mock.SetupProperty(x => x.StrategyTriggered, config.StrategyTriggered);
            mock.SetupProperty(x => x.TriggeredDirection, config.TriggeredDirection);

            return mock;
        }
    }
}
