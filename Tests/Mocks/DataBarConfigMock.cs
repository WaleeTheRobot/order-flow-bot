using Moq;
using NinjaTrader.Custom.AddOns.OrderFlowBot.Configs;
using OrderFlowBot.Tests.Mocks.Data;

namespace OrderFlowBot.Tests.Mocks
{
    public static class DataBarConfigMock
    {
        public static Mock<IDataBarConfig> CreateDataBarConfig()
        {
            var config = new DataBarConfigData();

            var mock = new Mock<IDataBarConfig>();
            mock.SetupGet(x => x.TickSize).Returns(config.TickSize);
            mock.SetupGet(x => x.TicksPerLevel).Returns(config.TicksPerLevel);
            mock.SetupGet(x => x.StackedImbalance).Returns(config.StackedImbalance);
            mock.SetupGet(x => x.ImbalanceRatio).Returns(config.ImbalanceRatio);
            mock.SetupGet(x => x.ImbalanceMinDelta).Returns(config.ImbalanceMinDelta);
            mock.SetupGet(x => x.ValueAreaPercentage).Returns(config.ValueAreaPercentage);

            return mock;
        }
    }
}
