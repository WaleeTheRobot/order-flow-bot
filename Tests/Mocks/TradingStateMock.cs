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
            mock.SetupGet(x => x.StandardInverse).Returns(config.StandardInverse);
            mock.SetupGet(x => x.IsTradingEnabled).Returns(config.IsTradingEnabled);
            mock.SetupGet(x => x.IsAutoTradeEnabled).Returns(config.IsAutoTradeEnabled);
            mock.SetupGet(x => x.IsAlertEnabled).Returns(config.IsAlertEnabled);
            mock.SetupGet(x => x.TriggerStrikePrice).Returns(config.TriggerStrikePrice);

            return mock;
        }
    }
}
