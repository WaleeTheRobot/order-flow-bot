using Moq;
using NinjaTrader.Custom.AddOns.OrderFlowBot.States;
using OrderFlowBot.Tests.Mocks.Data;

namespace OrderFlowBot.Tests.Mocks
{
    public static class TradingStateMock
    {
        public static Mock<TradingState> CreateDataBarConfig()
        {
            var config = new TradingStateData();

            var mock = new Mock<TradingState>();
            mock.SetupGet(x => x.TriggeredName).Returns(config.TriggeredName);
            mock.SetupGet(x => x.StrategyTriggered).Returns(config.StrategyTriggered);
            mock.SetupGet(x => x.TriggeredDirection).Returns(config.TriggeredDirection);
            mock.SetupGet(x => x.SelectedTradeDirection).Returns(config.SelectedTradeDirection);

            return mock;
        }
    }
}
